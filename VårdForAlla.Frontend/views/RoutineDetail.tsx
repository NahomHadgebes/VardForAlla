import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useRoutine, useDeleteRoutine } from '../hooks/useRoutines';
import Icon from '../components/Icon';
import { useToast } from '../context/ToastContext';
import { useAuth } from '../context/AuthContext';

const RoutineDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { showToast } = useToast();
  const { user } = useAuth();
  
  const numericId = id ? parseInt(id, 10) : undefined;
  const { data: routine, isLoading, error } = useRoutine(numericId);
  const deleteMutation = useDeleteRoutine();
  
  // FIXAT: Översättningsfunktionalitet från backend
  const [selectedLanguage, setSelectedLanguage] = useState('sv');

  const isAdmin = user?.role === 'Admin';

  const handleDelete = async () => {
    if (window.confirm('Är du helt säker på att du vill radera denna rutin permanent?')) {
      try {
        await deleteMutation.mutateAsync(numericId!);
        showToast('Rutinen raderades framgångsrikt.', 'info');
        navigate('/routines');
      } catch (err) {
        showToast('Kunde inte radera rutinen.', 'error');
      }
    }
  };

  // FIXAT: Funktion för att hämta översatt text från backend-data
  const getTranslatedText = (step: any, languageCode: string): string => {
    if (languageCode === 'sv') {
      return step.simpleText;
    }
    
    // Matcha språkkoder från backend: en, ar, so
    const translation = step.translations?.find((t: any) => 
      t.languageCode.toLowerCase() === languageCode.toLowerCase()
    );
    
    return translation?.translatedText || step.simpleText;
  };

  if (isLoading) return (
    <div className="flex flex-col items-center justify-center p-20 min-h-[60vh]">
      <div className="w-16 h-16 border-4 border-slate-900 border-t-transparent rounded-full animate-spin mb-6"></div>
      <p className="text-slate-900 font-black text-xl">Hämtar rutindetaljer...</p>
    </div>
  );

  if (error || !routine) return (
    <div className="max-w-2xl mx-auto my-20 p-10 text-center bg-red-50 border-2 border-red-100 rounded-[3rem] shadow-xl">
      <div className="w-20 h-20 bg-red-100 rounded-full flex items-center justify-center text-red-600 mx-auto mb-6">
        <svg className="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" /></svg>
      </div>
      <h2 className="text-3xl font-black text-red-900 mb-4 tracking-tight">Något gick fel</h2>
      <p className="text-red-700 font-bold mb-8">Kunde inte hitta den efterfrågade rutinen. Den kan ha raderats eller så saknas nätverksanslutning.</p>
      <button onClick={() => navigate('/routines')} className="px-10 py-4 bg-slate-900 text-white font-black rounded-2xl hover:bg-slate-800 transition-all">Gå till rutinlistan</button>
    </div>
  );

  return (
    <div className="max-w-5xl mx-auto space-y-10 animate-in fade-in duration-700">
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-6">
        <button 
          onClick={() => navigate('/routines')} 
          className="flex items-center gap-3 text-slate-900 hover:text-blue-600 transition-all font-black text-lg group"
        >
          <div className="w-10 h-10 rounded-xl bg-slate-100 flex items-center justify-center group-hover:bg-blue-50 transition-all">
            <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
            </svg>
          </div>
          Tillbaka till listan
        </button>
        
        {isAdmin && (
          <div className="flex gap-4">
            <button 
              onClick={handleDelete}
              disabled={deleteMutation.isPending}
              className="px-8 py-3.5 font-black text-red-600 bg-white border-2 border-red-50 hover:border-red-100 rounded-2xl hover:bg-red-50 transition-all shadow-sm flex items-center gap-3"
            >
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
              Radera rutin
            </button>
            <button 
              onClick={() => navigate(`/manage/${routine.id}`)}
              className="px-8 py-3.5 font-black text-white bg-slate-900 rounded-2xl hover:bg-blue-700 transition-all shadow-xl shadow-slate-200 flex items-center gap-3"
            >
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
              Redigera
            </button>
          </div>
        )}
      </div>

      <div className="bg-white p-10 md:p-14 rounded-[3.5rem] shadow-2xl shadow-slate-200/50 border border-slate-100 overflow-hidden relative">
        <div className="absolute top-0 right-0 w-64 h-64 bg-slate-50 rounded-bl-full -mr-20 -mt-20 opacity-80 pointer-events-none"></div>

        <header className="relative z-10 space-y-6 mb-14 border-b border-slate-100 pb-12">
          <div className="flex flex-wrap items-center gap-3">
            <span className="inline-block px-4 py-1.5 text-[11px] font-black text-blue-800 bg-blue-50 rounded-xl uppercase tracking-widest border border-blue-200 shadow-sm">
              {routine.category}
            </span>
            {routine.isTemplate && (
              <span className="inline-block px-4 py-1.5 text-[11px] font-black text-slate-900 bg-slate-100 rounded-xl uppercase tracking-widest border border-slate-200 shadow-sm">
                Systemmall
              </span>
            )}
          </div>
          <h1 className="text-5xl font-black text-slate-900 tracking-tighter leading-tight">{routine.title}</h1>
          <p className="text-xl text-slate-500 leading-relaxed font-bold max-w-3xl italic">
            {routine.description || "Ingen beskrivning tillgänglig"}
          </p>
          
          <div className="flex flex-wrap items-center gap-3 pt-4">
            <div className="flex items-center gap-2 text-xs font-black text-slate-400 uppercase tracking-widest mr-4">
              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
              Uppdaterad: {new Date(routine.updatedAt).toLocaleDateString('sv-SE')}
            </div>
            {(routine.tags || []).map(tag => (
              <span key={tag} className="px-3 py-1.5 bg-slate-50 text-slate-900 text-[10px] font-black rounded-lg border border-slate-100 shadow-sm">#{tag.toUpperCase()}</span>
            ))}
          </div>
        </header>

        <section className="space-y-10 relative z-10">
          <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-6 bg-slate-50 p-6 rounded-3xl border border-slate-100">
            <div className="flex items-center gap-4">
               <div className="w-1.5 h-8 bg-blue-600 rounded-full"></div>
               <h3 className="text-2xl font-black text-slate-900 tracking-tight">Instruktionssteg</h3>
            </div>
            
            <div className="flex items-center gap-4">
              <label className="text-xs font-black text-slate-500 uppercase tracking-widest whitespace-nowrap">Kommunikationsspråk:</label>
              <select 
                className="text-sm font-black bg-white border-2 border-slate-200 rounded-xl focus:ring-4 focus:ring-blue-100 focus:border-blue-500 cursor-pointer shadow-sm px-6 py-3 transition-all outline-none"
                value={selectedLanguage}
                onChange={(e) => setSelectedLanguage(e.target.value)}
              >
                <option value="sv">Svenska (Huvudspråk)</option>
                <option value="en">English (Engelska)</option>
                <option value="ar">العربية (Arabiska)</option>
                <option value="so">Af-Soomaali (Somaliska)</option>
              </select>
            </div>
          </div>

          <div className="grid gap-8">
            {routine.steps && routine.steps.length > 0 ? (
              routine.steps.sort((a, b) => a.order - b.order).map((step, index) => (
                <div key={step.id} className="flex flex-col md:flex-row gap-8 p-10 rounded-[3rem] bg-white border-2 border-slate-50 hover:border-slate-200 hover:shadow-2xl hover:shadow-slate-200/30 transition-all duration-500 group relative">
                  <div className="flex-shrink-0 flex md:flex-col items-center gap-6">
                    <div className="relative">
                      <div className="w-16 h-16 rounded-2xl bg-slate-900 text-white flex items-center justify-center font-black text-2xl shadow-xl group-hover:scale-110 transition-transform duration-300">
                        {index + 1}
                      </div>
                      <div className="absolute -bottom-2 -right-2 w-8 h-8 rounded-full bg-blue-600 text-white border-2 border-white flex items-center justify-center shadow-md">
                        <Icon iconKey={step.iconKey || 'default'} className="w-4 h-4" />
                      </div>
                    </div>
                  </div>
                  
                  <div className="flex-1 space-y-6">
                    <div className="space-y-2">
                      <div className="flex items-center gap-2">
                         <span className="text-[10px] font-black text-blue-600 uppercase tracking-widest">Patientinstruktion</span>
                         {selectedLanguage !== 'sv' && <span className="text-[10px] font-black text-slate-400 uppercase tracking-widest">• Översatt</span>}
                      </div>
                      <p className="text-2xl font-black text-slate-900 leading-snug tracking-tight">
                        {getTranslatedText(step, selectedLanguage) || 
                          <span className="text-slate-300 italic font-medium">Översättning saknas för detta steg.</span>
                        }
                      </p>
                    </div>
                    
                    <div className="pt-6 border-t border-slate-100 flex flex-col sm:flex-row sm:items-center gap-4">
                      <span className="text-[10px] font-black text-slate-400 uppercase tracking-widest whitespace-nowrap">Medicinsk Term:</span>
                      <p className="text-sm font-black text-slate-700 italic bg-slate-50 inline-block px-4 py-2 rounded-xl border border-slate-100 shadow-inner">
                        {step.originalText || step.simpleText}
                      </p>
                    </div>
                  </div>
                </div>
              ))
            ) : (
              <div className="text-center py-10 bg-slate-50 rounded-3xl">
                <p className="text-slate-400 font-medium">Inga steg definierade för denna rutin.</p>
              </div>
            )}
          </div>
        </section>

        <footer className="mt-16 pt-10 border-t border-slate-100 flex justify-center">
            <p className="text-xs text-slate-400 font-bold flex items-center gap-2 italic">
               <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" /></svg>
               Medicinsk säkerhetskopia: Innehåll granskat av behörig personal.
            </p>
        </footer>
      </div>
    </div>
  );
};

export default RoutineDetail;