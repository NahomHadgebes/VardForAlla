
/**
 * Authentication DTOs
 */
export interface UserDto {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: 'Admin' | 'User';
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterUserDto {
  firstName: string;
  lastName: string;
  email: string;
  password?: string;
}

export interface AuthResponseDto {
  token: string;
  user: UserDto;
}

/**
 * Routine DTOs
 */
export interface StepTranslationDto {
  id?: number;
  languageCode: string;
  translatedText: string;
}

export interface StepTranslationCreateDto {
  languageCode: string;
  translatedText: string;
}

export interface RoutineStepDto {
  id: number;
  order: number;
  simpleText: string;
  originalText: string;
  iconKey: string;
  translations: StepTranslationDto[];
}

export interface RoutineStepCreateDto {
  order: number;
  simpleText: string;
  originalText: string;
  iconKey: string;
  translations: StepTranslationCreateDto[];
}

export interface RoutineDto {
  id: number;
  title: string;
  description?: string;
  category: string;
  tags?: string[];
  steps: RoutineStepDto[];
  isTemplate?: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface RoutineListDto {
  id: number;
  title: string;
  description?: string;
  category: string;
  tags?: string[];
  isTemplate?: boolean;
}

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
}

export interface RoutineCreateDto {
  title: string;
  description?: string;
  category: string;
  tags: string[];
  steps: RoutineStepCreateDto[];
  isTemplate?: boolean;
}

export interface RoutineUpdateDto extends RoutineCreateDto {
  id: number;
}
