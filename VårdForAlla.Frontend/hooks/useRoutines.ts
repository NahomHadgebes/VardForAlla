import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../services/api';
import { RoutineDto, RoutineCreateDto, RoutineUpdateDto, StepTranslationCreateDto } from '../types';

/**
 * FIXAT: Hook för att hämta rutiner med korrekt sök/filter-stöd
 */
export const useRoutines = (search?: string, category?: string) => {
  return useQuery({
    queryKey: ['routines', search, category],
    queryFn: async () => {
      const params = new URLSearchParams();
      if (search) params.append('search', search);
      if (category) params.append('category', category);
      
      const queryString = params.toString();
      const url = queryString ? `/routine?${queryString}` : '/routine';
      
      const response = await api.get<RoutineDto[]>(url);
      return response.data;
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
      // Backend returnerar kategorier från rutiner
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

// FIXAT: Översättningsfunktion för steg
export const useAddTranslation = (stepId: number) => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: StepTranslationCreateDto) => 
      api.post(`/steps/${stepId}/translations`, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['routine'] });
    },
  });
};