import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Navigation: React.FC = () => {
  const { user, logout } = useAuth();
  const location = useLocation();

  const isAdmin = user?.role === 'Admin';

  // FIXAT: Alla användare kan nu skapa rutiner, admin får extra meny
  const navItems = isAdmin 
    ? [
        { path: '/', label: 'Översikt', icon: 'M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6' },
        { path: '/routines', label: 'Bibliotek', icon: 'M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2' },
        { path: '/manage', label: 'Administration', icon: 'M12 6V4m0 2a2 2 0 100 4m0-4a2 2 0 110 4m-6 8a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4m6 6v10m6-2a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4' },
      ]
    : [
        { path: '/', label: 'Mina Sidor', icon: 'M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6' },
        { path: '/routines', label: 'Sök Rutin', icon: 'M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z' },
        { path: '/manage', label: 'Skapa Ny', icon: 'M12 4v16m8-8H4' }, // FIXAT: Alla kan skapa
        { path: '/profile', label: 'Inställningar', icon: 'M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z' },
      ];

  return (
    <nav className="sticky top-0 z-50 flex items-center justify-between px-6 md:px-12 py-5 bg-white border-b border-slate-100 shadow-sm">
      <div className="flex items-center gap-10">
        {/* COOL MODERN VFA LOGO */}
        <Link to="/" className="flex items-center gap-4 group">
          <div className="relative w-11 h-11 bg-slate-900 rounded-2xl flex items-center justify-center overflow-hidden shadow-2xl transition-all duration-500 group-hover:bg-blue-600 group-hover:rotate-6">
            <div className="absolute inset-0 bg-gradient-to-tr from-white/10 to-transparent"></div>
            <span className="text-white text-lg font-black tracking-tighter relative z-10">VFA</span>
            <div className="absolute top-1 right-1 w-2 h-2 bg-blue-400 rounded-full animate-pulse"></div>
          </div>
          <div className="hidden lg:block border-l border-slate-100 pl-4">
            <h1 className="text-lg font-black text-slate-900 leading-none tracking-tight">VårdFörAlla</h1>
            <p className="text-[9px] font-black text-blue-600 uppercase tracking-widest mt-1">Medical Protocol OS</p>
          </div>
        </Link>
        
        <div className="flex gap-1.5">
          {navItems.map((item) => {
            const isActive = location.pathname === item.path;
            return (
              <Link
                key={item.path}
                to={item.path}
                className={`flex items-center gap-2.5 px-5 py-3 rounded-2xl text-sm font-black transition-all ${
                  isActive
                    ? 'text-slate-900 bg-slate-50 border border-slate-200 shadow-inner'
                    : 'text-slate-500 hover:bg-slate-50 hover:text-slate-900 border border-transparent'
                }`}
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d={item.icon} />
                </svg>
                <span className="hidden sm:inline">{item.label}</span>
              </Link>
            );
          })}
        </div>
      </div>

      <div className="flex items-center gap-6">
        <Link 
          to="/profile" 
          className="flex items-center gap-4 px-3 py-1.5 rounded-2xl hover:bg-slate-50 transition-all border border-transparent hover:border-slate-100 group"
        >
          <div className="text-right hidden md:block">
            <p className="text-sm font-black text-slate-900 leading-none group-hover:text-blue-600 transition-colors">{user?.firstName} {user?.lastName[0]}.</p>
            <p className="text-[9px] font-black uppercase tracking-[0.2em] text-slate-400 mt-1">
              {isAdmin ? 'Systemansvarig' : 'Klinisk Personal'}
            </p>
          </div>
          <div className="w-10 h-10 rounded-xl bg-slate-100 flex items-center justify-center text-slate-900 font-black text-sm border border-slate-200 shadow-sm group-hover:border-blue-200 group-hover:bg-white transition-all">
            {user?.firstName[0]}{user?.lastName[0]}
          </div>
        </Link>
        
        <div className="h-8 w-px bg-slate-100 hidden sm:block"></div>

        <button
          onClick={logout}
          className="p-3 text-slate-400 hover:text-red-600 hover:bg-red-50 rounded-2xl transition-all active:scale-90"
          title="Logga ut ur systemet"
        >
          <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
          </svg>
        </button>
      </div>
    </nav>
  );
};

export default Navigation;
