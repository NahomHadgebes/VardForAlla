
import React from 'react';
import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import api from '../services/api';
import { UserDto } from '../types';

const Profile: React.FC = () => {
  const { user, logout } = useAuth();
  
  const { data: profileData, isLoading } = useQuery({
    queryKey: ['profile', user?.id],
    queryFn: async () => {
      const response = await api.get<UserDto>('/auth/me');
      return response.data;
    },
    enabled: !!user,
  });

  const currentUser = profileData || user;

  if (isLoading) return <div className="p-20 text-center text-slate-900 font-black animate-pulse">Laddar din profil...</div>;

  return (
    <div className="max-w-5xl mx-auto space-y-10 animate-in fade-in slide-in-from-bottom-6 duration-700">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-6 border-b border-slate-200 pb-10">
        <div>
          <h1 className="text-5xl font-black text-slate-900 tracking-tight">Min Profil</h1>
          <p className="text-slate-500 font-bold mt-2 text-lg">Här ser du dina systeminställningar och behörigheter.</p>
        </div>
        <button 
          onClick={logout}
          className="flex items-center gap-3 px-8 py-4 rounded-2xl bg-red-600 text-white font-black hover:bg-red-700 transition-all shadow-xl shadow-red-100 active:scale-95"
        >
          <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" /></svg>
          Logga ut från systemet
        </button>
      </header>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-10">
        {/* Main User Info */}
        <div className="lg:col-span-2 space-y-10">
          <section className="bg-white p-10 rounded-[3rem] shadow-2xl shadow-slate-200/50 border border-slate-100 relative overflow-hidden">
            <div className="absolute top-0 right-0 w-40 h-40 bg-blue-50 rounded-bl-full -mr-10 -mt-10 opacity-60"></div>
            
            <div className="relative z-10 flex items-center gap-8 mb-12">
              <div className="w-28 h-28 rounded-[2.5rem] bg-slate-900 flex items-center justify-center text-white text-4xl font-black shadow-2xl shadow-slate-300">
                {currentUser?.firstName[0]}{currentUser?.lastName[0]}
              </div>
              <div className="space-y-1">
                <h2 className="text-3xl font-black text-slate-900 tracking-tight">{currentUser?.firstName} {currentUser?.lastName}</h2>
                <p className="text-blue-600 font-black text-lg">{currentUser?.email}</p>
                <div className="pt-2">
                  {currentUser?.role === 'Admin' ? (
                    <span className="inline-flex items-center px-4 py-1.5 rounded-xl bg-purple-900 text-white text-[11px] font-black uppercase tracking-widest border-2 border-purple-800">
                      Administratör
                    </span>
                  ) : (
                    <span className="inline-flex items-center px-4 py-1.5 rounded-xl bg-blue-600 text-white text-[11px] font-black uppercase tracking-widest border-2 border-blue-500">
                      Personal (Standard)
                    </span>
                  )}
                </div>
              </div>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
              <div className="p-6 bg-slate-50 rounded-3xl border border-slate-100 group hover:border-blue-200 transition-all">
                <p className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-1">Användar-ID</p>
                <p className="text-slate-900 font-bold font-mono text-sm">{currentUser?.id}</p>
              </div>
              <div className="p-6 bg-slate-50 rounded-3xl border border-slate-100 group hover:border-blue-200 transition-all">
                <p className="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-1">Inloggad sedan</p>
                <p className="text-slate-900 font-bold text-sm">{new Date().toLocaleDateString('sv-SE')}</p>
              </div>
            </div>
          </section>

          {/* Role Specific Actions */}
          {currentUser?.role === 'Admin' ? (
            <section className="bg-purple-900 p-10 rounded-[3rem] text-white shadow-2xl shadow-purple-200/50 relative overflow-hidden group">
              <div className="absolute top-0 right-0 w-64 h-64 bg-white/10 rounded-full -mr-32 -mt-32 blur-3xl group-hover:bg-white/20 transition-all"></div>
              <div className="relative z-10 space-y-6">
                <div className="flex items-center gap-4">
                  <div className="p-3 bg-white/10 rounded-2xl backdrop-blur-md">
                    <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" /></svg>
                  </div>
                  <h3 className="text-3xl font-black tracking-tight">Administratörsverktyg</h3>
                </div>
                <p className="text-purple-100 font-bold text-lg leading-relaxed max-w-xl">
                  Som administratör har du behörighet att hantera systemets alla globala mallar, översättningar och taggar.
                </p>
                <div className="flex flex-wrap gap-4 pt-4">
                  <Link to="/manage" className="px-8 py-4 bg-white text-purple-900 font-black rounded-2xl hover:bg-purple-50 transition-all shadow-xl active:scale-95">
                    Hantera systemmallar
                  </Link>
                  <button className="px-8 py-4 bg-purple-700/50 border-2 border-purple-500/50 text-white font-black rounded-2xl hover:bg-purple-700 transition-all active:scale-95">
                    Granska ändringslogg
                  </button>
                </div>
              </div>
            </section>
          ) : (
            <section className="bg-blue-600 p-10 rounded-[3rem] text-white shadow-2xl shadow-blue-200/50 relative overflow-hidden group">
              <div className="absolute top-0 right-0 w-64 h-64 bg-white/10 rounded-full -mr-32 -mt-32 blur-3xl group-hover:bg-white/20 transition-all"></div>
              <div className="relative z-10 space-y-6">
                <div className="flex items-center gap-4">
                  <div className="p-3 bg-white/10 rounded-2xl backdrop-blur-md">
                    <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
                  </div>
                  <h3 className="text-3xl font-black tracking-tight">Mina inställningar</h3>
                </div>
                <p className="text-blue-50 font-bold text-lg leading-relaxed max-w-xl">
                  Du har tillgång till alla rutiner och kan spara dina egna bokmärken för snabb åtkomst under arbetet.
                </p>
                <div className="flex flex-wrap gap-4 pt-4">
                  <Link to="/routines" className="px-8 py-4 bg-white text-blue-600 font-black rounded-2xl hover:bg-blue-50 transition-all shadow-xl active:scale-95">
                    Bläddra i rutiner
                  </Link>
                  <button className="px-8 py-4 bg-blue-500/50 border-2 border-blue-400/50 text-white font-black rounded-2xl hover:bg-blue-500 transition-all active:scale-95">
                    Ändra lösenord
                  </button>
                </div>
              </div>
            </section>
          )}
        </div>

        {/* Sidebar */}
        <div className="space-y-8">
          <section className="bg-white p-8 rounded-[2.5rem] border border-slate-200 shadow-xl shadow-slate-200/50 space-y-6">
            <h3 className="text-xs font-black text-slate-400 uppercase tracking-[0.2em] ml-1">Support & Hjälp</h3>
            <div className="space-y-4">
              <button className="w-full flex items-center gap-4 p-5 rounded-2xl bg-slate-50 border border-slate-100 hover:border-blue-200 hover:bg-white transition-all text-left">
                <div className="w-10 h-10 rounded-xl bg-white border border-slate-200 flex items-center justify-center text-blue-600 shadow-sm">
                  <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                </div>
                <div>
                  <p className="text-sm font-black text-slate-900">Användarguide</p>
                  <p className="text-xs text-slate-500 font-bold">Lär dig systemet</p>
                </div>
              </button>
              <a href="mailto:it@sjukhuset.se" className="w-full flex items-center gap-4 p-5 rounded-2xl bg-slate-50 border border-slate-100 hover:border-blue-200 hover:bg-white transition-all text-left">
                <div className="w-10 h-10 rounded-xl bg-white border border-slate-200 flex items-center justify-center text-blue-600 shadow-sm">
                  <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" /></svg>
                </div>
                <div>
                  <p className="text-sm font-black text-slate-900">IT-Support</p>
                  <p className="text-xs text-slate-500 font-bold">Få teknisk hjälp</p>
                </div>
              </a>
            </div>
          </section>
          
          <div className="bg-slate-900 p-8 rounded-[2.5rem] text-white shadow-2xl space-y-4">
             <h4 className="text-[10px] font-black text-slate-400 uppercase tracking-widest">Säkerhetsmeddelande</h4>
             <p className="text-xs text-slate-300 font-bold leading-relaxed">
               Dela aldrig dina inloggningsuppgifter med obehöriga. Kom ihåg att alltid logga ut när du lämnar din station.
             </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile;
