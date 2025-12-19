
import axios from 'axios';

const API_BASE_URL = 'https://localhost:7144/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Request Interceptor: Bifogar JWT-token till alla anrop om den finns i localStorage.
 */
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('vfa_token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

/**
 * Response Interceptor: Hanterar globala fel, specifikt 401 (Obehörig).
 */
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Om servern returnerar 401 Unauthorized, logga ut användaren och rensa sessionen
    if (error.response?.status === 401) {
      localStorage.removeItem('vfa_token');
      localStorage.removeItem('vfa_user');
      // Förhindra oändlig loop om vi redan är på inloggningssidan
      if (!window.location.hash.includes('/login')) {
        window.location.hash = '#/login';
      }
    }
    
    // Logga fel i produktion för lättare felsökning via konsolen
    console.error(`[API Error] ${error.config?.method?.toUpperCase()} ${error.config?.url}:`, error.response?.data || error.message);
    
    return Promise.reject(error);
  }
);

export default api;
