
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';
import { RoutineDto, RoutineCreateDto, RoutineUpdateDto, StepTranslationCreateDto } from '../types';

/**
 * Hook for fetching routines. Supports simple listing (isTemplate) 
 * and advanced search/pagination (for the Explorer view).
 */
// Added support for multiple arguments to resolve the argument mismatch error in Explorer.tsx
export const useRoutines = (pageOrTemplate?: number | boolean, pageSize?: number, search?: string, category?: string) => {
  return useQuery({
    queryKey: ['routines', pageOrTemplate, pageSize, search, category],
    queryFn: async () => {
      const params = new URLSearchParams();
      
      // Handle polymorphism: either isTemplate (boolean) or page (number)
      if (typeof pageOrTemplate === 'boolean') {
        params.append('isTemplate', pageOrTemplate.toString());
      } else if (typeof pageOrTemplate === 'number') {
        params.append('page', pageOrTemplate.toString());
        if (pageSize) params.append('pageSize', pageSize.toString());
        if (search) params.append('search', search);
        if (category) params.append('category', category);
      }

      const queryString = params.toString();
      const url = queryString ? `/routine?${queryString}` : '/routine';
      
      // Using any to handle different response structures from backend (flat array or paginated object)
      const response = await api.get<any>(url);
      const data = response.data;

      // If called from Explorer (page is a number) but backend returns a flat list,
      // emulate the paginated structure that Explorer expects.
      if (typeof pageOrTemplate === 'number' && Array.isArray(data)) {
        return {
          items: data,
          totalCount: data.length
        };
      }

      return data;
    },
  });
};

export const useRoutine = (id: number | string | undefined) => {
  return useQuery({
    queryKey: ['routine', id],
    queryFn: async () => {
      if (!id) return null;
      const response = await api.get<RoutineDto>(`/routine/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
};

export const useCategories = () => {
  return useQuery({
    queryKey: ['categories'],
    queryFn: async () => {
      // Backend lacks /categories endpoint, returns hardcoded list for clinical needs
      return ['Hjärta', 'Lunga', 'Diabetes', 'Akut', 'Allmänt'];
    }
  });
};

export const useCreateRoutine = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: RoutineCreateDto) => api.post('/routine', data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['routines'] });
    },
  });
};

export const useUpdateRoutine = (id: number) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: RoutineUpdateDto) => api.put(`/routine/${id}`, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['routines'] });
      queryClient.invalidateQueries({ queryKey: ['routine', id] });
    },
  });
};

export const useDeleteRoutine = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: number) => api.delete(`/routine/${id}`),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['routines'] });
    },
  });
};

export const useAddTranslation = (stepId: number) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: StepTranslationCreateDto) => api.post(`/routine/step/${stepId}/translation`, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['routine'] });
    },
  });
};
  