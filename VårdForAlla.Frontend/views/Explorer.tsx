
import React, { useState, useEffect } from 'react';
import { useRoutines } from '../hooks/useRoutines';
import RoutineCard from '../components/RoutineCard';

const CATEGORIES = [
  'Undersökning',
  'Behandling',
  'Hygien',
  'Medicinering',
  'Pre-operativ',
  'Post-operativ',
  'Akut'
];

const Explorer: React.FC = () => {
  const [search, setSearch] = useState('');
  const [category, setCategory] = useState('');
  const [page, setPage] = useState(1);
  const pageSize = 9;

  const { data, isLoading, isError } = useRoutines(page, pageSize, search, category);

  // Reset page when filters change
  useEffect(() => {
    setPage(1);
  }, [search, category]);

  const totalPages = data ? Math.ceil(data.totalCount / pageSize) : 0;

  return (
    <div className="max-w-7xl mx-auto space-y-8">
      <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
        <div className="space-y-1">
          <h2 className="text-3xl font-extrabold text-slate-900 tracking-tight">Utforska rutiner</h2>
          <p className="text-slate-500">Bläddra och sök i den kompletta katalogen av medicinska rutiner.</p>
        </div>
        
        <div className="flex flex-col sm:flex-row gap-4 w-full md:w-auto">
          {/* Category Filter */}
          <div className="w-full sm:w-48">
            <label className="block text-xs font-bold text-slate-400 uppercase mb-1.5 ml-1">Kategori</label>
            <select
              value={category}
              onChange={(e) => setCategory(e.target.value)}
              className="block w-full px-4 py-2.5 bg-white border border-slate-200 rounded-xl text-sm focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-shadow shadow-sm"
            >
              <option value="">Alla kategorier</option>
              {CATEGORIES.map(cat => (
                <option key={cat} value={cat}>{cat}</option>
              ))}
            </select>
          </div>

          {/* Search Input */}
          <div className="relative flex-1 sm:w-80">
            <label className="block text-xs font-bold text-slate-400 uppercase mb-1.5 ml-1">Sökord</label>
            <div className="relative">
              <span className="absolute inset-y-0 left-0 pl-3.5 flex items-center pointer-events-none text-slate-400">
                <svg className="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
              </span>
              <input
                type="text"
                className="block w-full pl-11 pr-4 py-2.5 bg-white border border-slate-200 rounded-xl text-sm placeholder-slate-400 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-shadow shadow-sm"
                placeholder="Sök på titel eller innehåll..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
              />
            </div>
          </div>
        </div>
      </div>

      {isError && (
        <div className="bg-red-50 border border-red-100 p-6 rounded-2xl text-center">
          <p className="text-red-700 font-medium">Ett fel uppstod vid hämtning av rutiner. Vänligen försök igen senare.</p>
        </div>
      )}

      {isLoading ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {[...Array(6)].map((_, i) => (
            <div key={i} className="bg-white p-6 rounded-xl border border-slate-100 shadow-sm animate-pulse space-y-4">
              <div className="h-4 bg-slate-100 rounded w-1/4"></div>
              <div className="h-6 bg-slate-100 rounded w-3/4"></div>
              <div className="h-20 bg-slate-100 rounded w-full"></div>
              <div className="flex gap-2">
                <div className="h-4 bg-slate-100 rounded w-12"></div>
                <div className="h-4 bg-slate-100 rounded w-12"></div>
              </div>
            </div>
          ))}
        </div>
      ) : data?.items.length === 0 ? (
        <div className="py-20 text-center bg-white rounded-2xl border border-dashed border-slate-300">
          <div className="inline-flex items-center justify-center w-16 h-16 rounded-full bg-slate-50 text-slate-300 mb-4">
            <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9.172 9.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
          <h3 className="text-lg font-bold text-slate-800">Inga rutiner hittades</h3>
          <p className="text-slate-500 mt-1">Prova att justera din sökning eller filter.</p>
          {(search || category) && (
            <button 
              onClick={() => { setSearch(''); setCategory(''); }}
              className="mt-4 text-blue-600 font-semibold hover:underline"
            >
              Rensa alla filter
            </button>
          )}
        </div>
      ) : (
        <>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {data?.items.map((routine) => (
              <RoutineCard key={routine.id} routine={routine} />
            ))}
          </div>

          {/* Pagination */}
          {totalPages > 1 && (
            <div className="flex flex-col items-center gap-4 pt-10">
              <div className="flex items-center bg-white border border-slate-200 rounded-2xl p-1 shadow-sm">
                <button
                  onClick={() => setPage(p => Math.max(1, p - 1))}
                  disabled={page === 1}
                  className="p-2 rounded-xl text-slate-500 hover:bg-slate-50 hover:text-blue-600 disabled:opacity-30 disabled:hover:bg-transparent transition-colors"
                  aria-label="Föregående sida"
                >
                  <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15 19l-7-7 7-7" />
                  </svg>
                </button>
                
                <div className="px-6 flex items-center gap-2">
                  <span className="text-sm font-bold text-slate-900">Sida {page}</span>
                  <span className="text-sm text-slate-400">av {totalPages}</span>
                </div>

                <button
                  onClick={() => setPage(p => Math.min(totalPages, p + 1))}
                  disabled={page === totalPages}
                  className="p-2 rounded-xl text-slate-500 hover:bg-slate-50 hover:text-blue-600 disabled:opacity-30 disabled:hover:bg-transparent transition-colors"
                  aria-label="Nästa sida"
                >
                  <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 5l7 7-7 7" />
                  </svg>
                </button>
              </div>
              <p className="text-xs text-slate-400 font-medium">Visar {data.items.length} av totalt {data.totalCount} rutiner</p>
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default Explorer;
