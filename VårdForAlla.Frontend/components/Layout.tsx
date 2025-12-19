
import React from 'react';
import { Outlet } from 'react-router-dom';
import { useIsFetching } from '@tanstack/react-query';
import Navigation from './Navigation';

const Layout: React.FC = () => {
  const isFetching = useIsFetching();

  return (
    <div className="min-h-screen flex flex-col bg-slate-50 text-slate-900">
      {/* Global Loading Bar */}
      <div className={`fixed top-0 left-0 right-0 h-1 bg-blue-600 z-[100] transition-transform duration-500 origin-left ${isFetching ? 'scale-x-100' : 'scale-x-0'}`}></div>

      <Navigation />
      
      <main className="flex-1 p-6 md:p-10 relative">
        <div className="max-w-7xl mx-auto">
          <Outlet />
        </div>
      </main>

      <footer className="py-12 px-8 border-t border-slate-200 bg-white">
        <div className="max-w-7xl mx-auto flex flex-col md:flex-row justify-between items-center gap-8">
          <div className="flex items-center gap-4">
             <div className="w-12 h-12 bg-slate-900 rounded-2xl flex items-center justify-center text-white font-black text-xs shadow-lg">VFA</div>
             <div className="text-slate-500 text-xs font-bold leading-tight">
               <p className="text-slate-900 font-black text-sm">VårdFörAlla System</p>
               <p>© {new Date().getFullYear()} Regionens IT-stöd • Produktionsmiljö</p>
             </div>
          </div>
          <div className="flex gap-10 text-[10px] text-slate-400 font-black uppercase tracking-[0.2em]">
            <a href="#" className="hover:text-blue-600 transition-colors">Säkerhetspolicy</a>
            <a href="#" className="hover:text-blue-600 transition-colors">Systemloggar</a>
            <a href="#" className="hover:text-blue-600 transition-colors">IT-Support</a>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default Layout;
