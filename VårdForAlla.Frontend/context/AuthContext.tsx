
import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { UserDto, LoginDto, RegisterUserDto, AuthResponseDto } from '../types';
import api from '../services/api';

interface AuthContextType {
  user: UserDto | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (data: LoginDto) => Promise<void>;
  register: (data: RegisterUserDto) => Promise<void>;
  logout: () => void;
  error: string | null;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<UserDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const verifySession = async () => {
      const token = localStorage.getItem('vfa_token');
      
      if (!token) {
        setIsLoading(false);
        return;
      }

      try {
        // Verifiera token mot backend
        const response = await api.get<UserDto>('https://localhost:7144/api/auth/me');
        setUser(response.data);
        localStorage.setItem('vfa_user', JSON.stringify(response.data));
      } catch (err) {
        console.error("Sessionen är ogiltig eller backend är nere.");
        logout();
      } finally {
        setIsLoading(false);
      }
    };

    verifySession();
  }, []);

  const login = async (data: LoginDto) => {
    setError(null);
    try {
      const response = await api.post<AuthResponseDto>('/auth/login', data);
      const { token, user: userData } = response.data;
      
      localStorage.setItem('vfa_token', token);
      localStorage.setItem('vfa_user', JSON.stringify(userData));
      setUser(userData);
    } catch (err: any) {
      const serverMsg = err.response?.data?.message || 'Inloggningen misslyckades.';
      setError(serverMsg);
      throw err;
    }
  };

  const register = async (data: RegisterUserDto) => {
    setError(null);
    try {
      await api.post('/auth/register', data);
    } catch (err: any) {
      const msg = err.response?.data?.message || 'Registreringen misslyckades.';
      setError(msg);
      throw err;
    }
  };

  const logout = () => {
    localStorage.removeItem('vfa_token');
    localStorage.removeItem('vfa_user');
    setUser(null);
    // Vi använder HashRouter, så vi navigerar till login
    window.location.hash = '#/login';
  };

  return (
    <AuthContext.Provider value={{ 
      user, 
      isAuthenticated: !!user, 
      isLoading, 
      login, 
      register, 
      logout,
      error 
    }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
