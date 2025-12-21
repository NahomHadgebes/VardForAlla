import React, { useState } from 'react';
import { useRoutines, useCategories } from '../hooks/useRoutines';
import RoutineCard from '../components/RoutineCard';

const RoutineList: React.FC = () => {
  const [search, setSearch] = useState('');
  const [category, setCategory] = useState('');

  const { data: categories } = useCategories();
  
  // FIXAT: Skicka sök/filter-parametrar direkt till API
  const { data: routines, isLoading, isError } = useRoutines(search, category);

  return (
    <div className="max-w-7xl mx-auto space-y-8 animate-in fade-in duration-500">
      <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
        <div>
          <h2 className="text-4xl font-black text-slate-900 tracking-tight">Rutinbibliotek</h2>
          <p className="text-slate-500 font-bold mt-1">Bläddra bland alla tillgängliga medicinska rutiner i systemet.</p>
        </div>
        
        <div className="flex flex-col sm:flex-row gap-3 w-full md:w-auto">
          <div className="space-y-1.5">
            <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1">Kategori</label>
            <select
              value={category}
              onChange={(e) => setCategory(e.target.value)}
              className="w-full px-5 py-3.5 bg-white border-2 border-slate-100 rounded-2xl text-sm font-black focus:ring-4 focus:ring-blue-50 outline-none shadow-sm cursor-pointer"
            >
              <option value="">Alla Kategorier</option>
              {categories?.map((cat) => (
                <option key={cat} value={cat}>{cat}</option>
              ))}
            </select>
          </div>
          
          <div className="flex-1 sm:w-80 space-y-1.5">
            <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1">Sök på titel</label>
            <div className="relative">
              <span className="absolute inset-y-0 left-0 pl-4 flex items-center pointer-events-none text-slate-400">
                <svg className="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
              </span>
              <input
                type="text"
                placeholder="Skriv för att söka..."
                className="block w-full pl-12 pr-5 py-3.5 bg-white border-2 border-slate-100 rounded-2xl text-sm font-black focus:ring-4 focus:ring-blue-50 outline-none shadow-sm transition-all"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
              />
            </div>
          </div>
        </div>
      </div>

      {isLoading ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {[...Array(6)].map((_, i) => (
            <div key={i} className="h-64 bg-white border-2 border-slate-50 rounded-[2.5rem] animate-pulse"></div>
          ))}
        </div>
      ) : isError ? (
        <div className="text-center py-24 bg-red-50 rounded-[3rem] border-2 border-red-100 space-y-4">
          <div className="w-16 h-16 bg-red-100 text-red-600 rounded-full flex items-center justify-center mx-auto">
            <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" /></svg>
          </div>
          <p className="text-red-900 font-black text-lg">Kunde inte hämta rutinerna.</p>
          <button onClick={() => window.location.reload()} className="px-8 py-3 bg-red-600 text-white font-black rounded-xl">Försök igen</button>
        </div>
      ) : !routines || routines.length === 0 ? (
        <div className="text-center py-32 bg-white rounded-[4rem] border-4 border-dashed border-slate-100">
          <p className="text-slate-400 font-black text-xl italic">Inga rutiner matchar din sökning.</p>
          <button onClick={() => { setSearch(''); setCategory(''); }} className="mt-4 text-blue-600 font-black hover:underline">Rensa alla filter</button>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {routines.map((routine) => (
            <RoutineCard key={routine.id} routine={routine} />
          ))}
        </div>
      )}
    </div>
  );
};

export default RoutineList;
