import React from 'react';
import { HashRouter, Routes, Route, Navigate } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { AuthProvider } from './context/AuthContext';
import { ToastProvider } from './context/ToastContext';
import Layout from './components/Layout';
import Login from './views/Login';
import Dashboard from './views/Dashboard';
import RoutineList from './views/RoutineList';
import RoutineDetail from './views/RoutineDetail';
import Management from './views/Management';
import Profile from './views/Profile';
import ProtectedRoute from './components/ProtectedRoute';
import ScrollToTop from './components/ScrollToTop';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
});

const App: React.FC = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <ToastProvider>
        <AuthProvider>
          <HashRouter>
            <ScrollToTop />
            
            <Routes>
              <Route path="/login" element={<Login />} />
              
              <Route path="/" element={<ProtectedRoute><Layout /></ProtectedRoute>}>
                <Route index element={<Dashboard />} />
                <Route path="routines" element={<RoutineList />} />
                <Route path="routine/:id" element={<RoutineDetail />} />
                <Route path="profile" element={<Profile />} />
                
                {/* Administrativa rutter - Nu tillgängliga utan roll-krav */}
                <Route path="manage" element={
                  <ProtectedRoute>
                    <Management />
                  </ProtectedRoute>
                } />
                <Route path="manage/:id" element={
                  <ProtectedRoute>
                    <Management />
                  </ProtectedRoute>
                } />
              </Route>

              <Route path="*" element={<Navigate to="/" />} />
            </Routes>
          </HashRouter>
        </AuthProvider>
      </ToastProvider>
    </QueryClientProvider>
  );
};

export default App;