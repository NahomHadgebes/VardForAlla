
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Login: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const { login, error } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    
    try {
      await login({ email, password });
      navigate('/');
    } catch (err) {
      console.error("Login attempt failed");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-slate-100 px-4">
      <div className="max-w-md w-full space-y-8 bg-white p-8 md:p-12 rounded-[3rem] shadow-2xl border border-slate-200">
        <div className="text-center">
          <div className="inline-block p-6 rounded-3xl bg-slate-900 text-white mb-6 shadow-xl">
            <span className="text-2xl font-black tracking-tighter">VFA</span>
          </div>
          <h2 className="text-4xl font-black text-slate-900 tracking-tight">VårdFörAlla</h2>
          <p className="mt-3 text-slate-500 font-bold uppercase text-[10px] tracking-[0.2em]">Logga in på klientsystemet</p>
        </div>

        <form className="mt-10 space-y-6" onSubmit={handleSubmit}>
          {error && (
            <div className="bg-red-50 text-red-700 p-5 rounded-2xl text-sm border-2 border-red-100 flex items-start gap-4">
              <svg className="w-6 h-6 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
                <path fillRule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7 4a1 1 0 11-2 0 1 1 0 012 0zm-1-9a1 1 0 00-1 1v4a1 1 0 102 0V6a1 1 0 00-1-1z" clipRule="evenodd" />
              </svg>
              <span className="font-bold leading-tight">{error}</span>
            </div>
          )}

          <div className="space-y-5">
            <div className="space-y-2">
              <label htmlFor="email" className="text-xs font-black text-slate-500 uppercase tracking-[0.2em] ml-2">E-postadress</label>
              <input
                id="email"
                type="email"
                required
                className="block w-full px-6 py-4 bg-slate-50 border-2 border-slate-100 rounded-2xl text-slate-900 font-black placeholder-slate-300 focus:outline-none focus:ring-4 focus:ring-blue-100 focus:border-slate-900 focus:bg-white transition-all"
                placeholder="namn@sjukhus.se"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
            <div className="space-y-2">
              <label htmlFor="password" className="text-xs font-black text-slate-500 uppercase tracking-[0.2em] ml-2">Lösenord</label>
              <input
                id="password"
                type="password"
                required
                className="block w-full px-6 py-4 bg-slate-50 border-2 border-slate-100 rounded-2xl text-slate-900 font-black placeholder-slate-300 focus:outline-none focus:ring-4 focus:ring-blue-100 focus:border-slate-900 focus:bg-white transition-all"
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className="w-full flex justify-center py-5 px-4 border border-transparent rounded-[2rem] shadow-2xl shadow-slate-200 text-base font-black text-white bg-slate-900 hover:bg-blue-600 focus:outline-none focus:ring-4 focus:ring-blue-500 transition-all active:scale-95 disabled:opacity-50"
          >
            {isSubmitting ? 'Verifierar behörighet...' : 'Logga in'}
          </button>
        </form>
        
        <div className="mt-8 text-center">
          <p className="text-xs text-slate-400 font-bold leading-relaxed">
            Detta system är endast för behörig vårdpersonal. Alla inloggningsförsök loggas.
          </p>
        </div>
      </div>
    </div>
  );
};

export default Login;
