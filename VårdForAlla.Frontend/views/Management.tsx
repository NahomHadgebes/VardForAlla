
import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useForm, useFieldArray } from 'react-hook-form';
import { useRoutine, useCreateRoutine, useUpdateRoutine, useCategories } from '../hooks/useRoutines';
import { RoutineCreateDto, RoutineUpdateDto } from '../types';
import { useToast } from '../context/ToastContext';

const Management: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { showToast } = useToast();
  
  const numericId = id ? parseInt(id, 10) : undefined;
  const { data: existingRoutine, isLoading: isRoutineLoading } = useRoutine(numericId);
  const { data: categories } = useCategories();
  const createMutation = useCreateRoutine();
  const updateMutation = useUpdateRoutine(numericId || 0);
  const [tagInput, setTagInput] = useState('');

  const { register, control, handleSubmit, reset, watch, setValue, formState: { errors } } = useForm<RoutineCreateDto>({
    defaultValues: {
      title: '',
      description: '',
      category: '',
      tags: [],
      steps: [],
      isTemplate: false
    }
  });

  const { fields, append, remove, move } = useFieldArray({
    control,
    name: "steps"
  });

  const currentTags = watch('tags') || [];

  // Populate form if editing an existing routine
  useEffect(() => {
    if (existingRoutine) {
      reset({
        title: existingRoutine.title,
        description: existingRoutine.description || '',
        category: existingRoutine.category,
        tags: existingRoutine.tags || [],
        steps: (existingRoutine.steps || []).map(s => ({
          order: s.order,
          simpleText: s.simpleText,
          originalText: s.originalText,
          iconKey: s.iconKey || 'default',
          translations: [
            { languageCode: 'en', translatedText: s.translations.find(t => t.languageCode === 'en')?.translatedText || '' },
            { languageCode: 'ar', translatedText: s.translations.find(t => t.languageCode === 'ar')?.translatedText || '' },
            { languageCode: 'so', translatedText: s.translations.find(t => t.languageCode === 'so')?.translatedText || '' }
          ]
        })),
        isTemplate: existingRoutine.isTemplate || false
      });
    } else {
        // Initialize with one empty step for creation
        if (fields.length === 0) {
            addNewStep();
        }
    }
  }, [existingRoutine, reset]);

  const onFormSubmit = async (data: RoutineCreateDto) => {
    if (data.steps.length === 0) {
      showToast('En rutin måste innehålla minst ett instruktionssteg.', 'error');
      return;
    }

    try {
      const payload = {
        title: data.title,
        description: data.description || '',
        category: data.category,
        tags: data.tags || [],
        isTemplate: !!data.isTemplate,
        steps: data.steps.map((s, idx) => ({ 
          order: idx + 1,
          simpleText: s.simpleText,
          originalText: s.originalText,
          iconKey: s.iconKey || 'default',
          translations: s.translations.filter(t => t.translatedText.trim() !== '').map(t => ({
            languageCode: t.languageCode,
            translatedText: t.translatedText
          }))
        }))
      };

      if (numericId) {
        await updateMutation.mutateAsync({ ...payload, id: numericId } as RoutineUpdateDto);
        showToast('Ändringarna har sparats i systemet.', 'success');
      } else {
        await createMutation.mutateAsync(payload);
        showToast('En ny medicinsk rutin har skapats.', 'success');
      }
      navigate('/routines');
    } catch (err: any) {
      const errorMsg = err.response?.data?.message || 'Kunde inte spara rutinen. Kontrollera anslutningen till servern.';
      showToast(errorMsg, 'error');
    }
  };

  const onFormError = () => {
    showToast('Vänligen fyll i alla obligatoriska fält korrekt.', 'error');
  };

  const handleAddTag = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && tagInput.trim()) {
      e.preventDefault();
      const newTag = tagInput.trim();
      if (!currentTags.includes(newTag)) {
        setValue('tags', [...currentTags, newTag]);
      }
      setTagInput('');
    }
  };

  const removeTag = (tagToRemove: string) => {
    setValue('tags', currentTags.filter(t => t !== tagToRemove));
  };

  const addNewStep = () => {
    append({ 
      order: fields.length + 1, 
      simpleText: '', 
      originalText: '', 
      iconKey: 'default', 
      translations: [
        { languageCode: 'en', translatedText: '' }, 
        { languageCode: 'ar', translatedText: '' }, 
        { languageCode: 'so', translatedText: '' }
      ] 
    });
  };

  if (id && isRoutineLoading) return (
    <div className="flex flex-col items-center justify-center p-20 min-h-[50vh]">
      <div className="w-12 h-12 border-4 border-slate-900 border-t-transparent rounded-full animate-spin mb-4"></div>
      <p className="text-slate-900 font-black">Laddar rutindata...</p>
    </div>
  );

  return (
    <div className="max-w-5xl mx-auto pb-24 animate-in fade-in duration-500">
      <header className="flex flex-col md:flex-row md:items-center justify-between mb-12 gap-6">
        <div>
          <h1 className="text-5xl font-black text-slate-900 tracking-tight">
            {id ? 'Redigera Rutin' : 'Ny Medicinsk Rutin'}
          </h1>
          <p className="text-slate-500 font-bold mt-2 text-lg italic">Konfigurera steg, medicinska termer och översättningar.</p>
        </div>
        <button
          type="button"
          onClick={() => navigate(-1)}
          className="px-8 py-4 text-sm font-black text-slate-500 hover:text-slate-900 transition-all border-2 border-slate-200 rounded-2xl bg-white hover:bg-slate-50 shadow-sm"
        >
          Avbryt
        </button>
      </header>

      <form onSubmit={handleSubmit(onFormSubmit, onFormError)} className="space-y-12">
        {/* Core Metadata */}
        <section className="bg-white p-10 rounded-[3rem] shadow-2xl shadow-slate-200/50 border border-slate-100 space-y-10">
          <div className="flex items-center gap-4">
            <div className="w-12 h-12 rounded-2xl bg-slate-900 flex items-center justify-center text-white shadow-lg">
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" /></svg>
            </div>
            <h2 className="text-2xl font-black text-slate-900 tracking-tight">Grundinformation</h2>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-10">
            <div className="space-y-3">
              <label className="text-xs font-black text-slate-400 uppercase tracking-widest ml-1">Titel <span className="text-red-500">*</span></label>
              <input
                {...register('title', { required: 'Titel är obligatorisk' })}
                placeholder="Namn på rutinen..."
                className={`w-full px-6 py-4.5 bg-slate-50 border-2 ${errors.title ? 'border-red-400 bg-red-50' : 'border-slate-100'} rounded-2xl focus:ring-4 focus:ring-blue-100 focus:bg-white outline-none transition-all font-black text-slate-900 placeholder-slate-300`}
              />
              {errors.title && <p className="text-red-600 text-xs font-black ml-1 uppercase">{errors.title.message}</p>}
            </div>

            <div className="space-y-3">
              <label className="text-xs font-black text-slate-400 uppercase tracking-widest ml-1">Kategori <span className="text-red-500">*</span></label>
              <select
                {...register('category', { required: 'Välj en kategori' })}
                className={`w-full px-6 py-4.5 bg-slate-50 border-2 ${errors.category ? 'border-red-400 bg-red-50' : 'border-slate-100'} rounded-2xl focus:ring-4 focus:ring-blue-100 focus:bg-white outline-none transition-all font-black text-slate-900 appearance-none cursor-pointer`}
              >
                <option value="">Välj kategori...</option>
                {(categories || ['Hjärta', 'Lunga', 'Diabetes', 'Akut', 'Allmänt']).map(cat => <option key={cat} value={cat}>{cat}</option>)}
              </select>
              {errors.category && <p className="text-red-600 text-xs font-black ml-1 uppercase">{errors.category.message}</p>}
            </div>
          </div>

          <div className="space-y-3">
            <label className="text-xs font-black text-slate-400 uppercase tracking-widest ml-1">Beskrivning</label>
            <textarea
              {...register('description')}
              placeholder="Kort sammanfattning av rutinens syfte..."
              className="w-full px-6 py-5 bg-slate-50 border-2 border-slate-100 rounded-3xl focus:ring-4 focus:ring-blue-100 focus:bg-white outline-none transition-all font-bold text-slate-900 h-32 resize-none shadow-inner"
            />
          </div>

          <div className="space-y-5">
            <label className="text-xs font-black text-slate-400 uppercase tracking-widest ml-1">Taggar (Tryck Enter)</label>
            <div className="flex flex-wrap gap-2">
              {currentTags.map(tag => (
                <span key={tag} className="flex items-center gap-2 px-4 py-2 bg-slate-900 text-white text-[11px] font-black rounded-xl shadow-md transition-all">
                  #{tag.toUpperCase()}
                  <button type="button" onClick={() => removeTag(tag)} className="text-slate-400 hover:text-red-400">
                    <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M6 18L18 6M6 6l12 12" /></svg>
                  </button>
                </span>
              ))}
            </div>
            <input
              value={tagInput}
              onChange={e => setTagInput(e.target.value)}
              onKeyDown={handleAddTag}
              placeholder="Skriv tagg och tryck Enter..."
              className="w-full px-6 py-4 bg-slate-50 border-2 border-slate-100 rounded-2xl focus:ring-4 focus:ring-blue-100 focus:bg-white outline-none transition-all font-bold text-slate-900"
            />
          </div>

          <div 
            className="flex items-center gap-4 p-6 bg-slate-50 rounded-[2rem] border-2 border-slate-100 group hover:border-slate-900 transition-all cursor-pointer select-none" 
            onClick={() => setValue('isTemplate', !watch('isTemplate'))}
          >
            <div className={`w-7 h-7 rounded-lg border-2 flex items-center justify-center transition-all ${watch('isTemplate') ? 'bg-slate-900 border-slate-900 shadow-lg' : 'bg-white border-slate-300'}`}>
              {watch('isTemplate') && <svg className="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="4" d="M5 13l4 4L19 7" /></svg>}
            </div>
            <p className="text-slate-900 font-black tracking-tight">Markera som systemmall (Template)</p>
          </div>
        </section>

        {/* Dynamic Instruction Steps */}
        <section className="space-y-8">
          <div className="flex items-center justify-between px-6">
            <div className="flex items-center gap-4">
              <div className="w-1.5 h-10 bg-blue-600 rounded-full"></div>
              <h3 className="text-3xl font-black text-slate-900 tracking-tight">Instruktionssteg</h3>
            </div>
            <button
              type="button"
              onClick={addNewStep}
              className="flex items-center gap-3 px-8 py-4 bg-slate-900 text-white font-black rounded-2xl hover:bg-blue-600 shadow-2xl transition-all active:scale-95 group"
            >
              <svg className="w-5 h-5 group-hover:rotate-90 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 4v16m8-8H4" /></svg>
              Lägg till Moment
            </button>
          </div>

          <div className="space-y-10">
            {fields.map((field, index) => (
              <div key={field.id} className="relative bg-white p-10 rounded-[3rem] border-2 border-slate-100 shadow-xl hover:shadow-2xl transition-all group">
                <div className="absolute -left-5 top-10 w-14 h-14 rounded-2xl bg-slate-900 text-white flex items-center justify-center font-black text-2xl shadow-xl z-10 border-4 border-white">
                  {index + 1}
                </div>

                <div className="absolute right-8 top-8 flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                   <button type="button" onClick={() => move(index, index - 1)} disabled={index === 0} className="p-3 bg-slate-100 text-slate-900 rounded-xl hover:bg-slate-200 disabled:opacity-20"><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M5 15l7-7 7 7" /></svg></button>
                   <button type="button" onClick={() => move(index, index + 1)} disabled={index === fields.length - 1} className="p-3 bg-slate-100 text-slate-900 rounded-xl hover:bg-slate-200 disabled:opacity-20"><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M19 9l-7 7-7-7" /></svg></button>
                   <button type="button" onClick={() => remove(index)} className="p-3 bg-red-100 text-red-700 rounded-xl hover:bg-red-200"><svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg></button>
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-2 gap-10 mt-8">
                  <div className="space-y-4">
                    <label className="text-[11px] font-black text-blue-600 uppercase tracking-widest">Enkel text (För patienten) <span className="text-red-500">*</span></label>
                    <input
                      {...register(`steps.${index}.simpleText` as const, { required: true })}
                      placeholder="Instruktion på klarspråk..."
                      className="w-full px-6 py-5 bg-slate-50 border-2 border-slate-100 rounded-2xl focus:ring-4 focus:ring-blue-50 focus:bg-white outline-none transition-all font-black text-slate-900 text-lg shadow-inner"
                    />
                  </div>
                  <div className="space-y-4">
                    <label className="text-[11px] font-black text-slate-400 uppercase tracking-widest">Medicinsk Term <span className="text-red-500">*</span></label>
                    <input
                      {...register(`steps.${index}.originalText` as const, { required: true })}
                      placeholder="Den medicinskt korrekta termen..."
                      className="w-full px-6 py-5 bg-slate-50 border-2 border-slate-100 rounded-2xl focus:ring-4 focus:ring-slate-100 focus:bg-white outline-none transition-all font-bold italic text-slate-500 text-lg"
                    />
                  </div>
                </div>

                <div className="mt-8 grid grid-cols-1 md:grid-cols-2 gap-6 items-end">
                   <div className="space-y-4">
                    <label className="text-[11px] font-black text-slate-400 uppercase tracking-widest">Ikon-nyckel</label>
                    <select 
                      {...register(`steps.${index}.iconKey` as const)}
                      className="w-full px-6 py-4 bg-slate-50 border-2 border-slate-100 rounded-2xl focus:ring-4 focus:ring-slate-100 focus:bg-white outline-none font-black text-slate-900 appearance-none cursor-pointer shadow-sm"
                    >
                      <option value="default">Standard Ikon</option>
                      <option value="wash">Hygien</option>
                      <option value="pill">Läkemedel</option>
                      <option value="fasting">Fasta</option>
                      <option value="trash">Kassering</option>
                      <option value="clean">Desinfektion</option>
                    </select>
                  </div>
                </div>

                {/* Translation Grid */}
                <div className="mt-10 pt-10 border-t-2 border-slate-50">
                  <h4 className="text-sm font-black text-slate-900 uppercase tracking-widest mb-6 flex items-center gap-3">
                    <svg className="w-5 h-5 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M3 5h12M9 3v2m1.048 9.5a18.022 18.022 0 01-3.837-5.613m7.584 5.613a18.022 18.022 0 00-3.837-5.613m3.837 5.613l-4.176-4.176m4.176 4.176l4.176 4.176M6.421 9a11.353 11.353 0 011.048 9.5" /></svg>
                    Språköversättningar
                  </h4>
                  <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    {['en', 'ar', 'so'].map((lang, langIdx) => (
                      <div key={lang} className="space-y-2">
                        <div className="flex items-center justify-between px-1">
                          <label className="text-[10px] font-black text-slate-400 uppercase tracking-widest">{lang === 'en' ? 'Engelska' : lang === 'ar' ? 'Arabiska' : 'Somaliska'}</label>
                          <span className="text-[9px] font-black text-blue-600 bg-blue-50 px-2 py-0.5 rounded-lg border border-blue-100">{lang.toUpperCase()}</span>
                        </div>
                        <textarea
                          placeholder={`Översättning för ${lang}...`}
                          className="w-full px-4 py-3 bg-slate-50 border-2 border-slate-100 rounded-xl text-sm font-bold text-slate-900 focus:ring-4 focus:ring-slate-100 focus:bg-white outline-none transition-all h-24 resize-none shadow-sm"
                          {...register(`steps.${index}.translations.${langIdx}.translatedText` as const)}
                        />
                        <input type="hidden" {...register(`steps.${index}.translations.${langIdx}.languageCode` as const)} defaultValue={lang} />
                      </div>
                    ))}
                  </div>
                </div>
              </div>
            ))}

            {fields.length === 0 && (
              <div 
                className="py-24 bg-slate-50 border-4 border-dashed border-slate-200 rounded-[4rem] text-center flex flex-col items-center group cursor-pointer hover:bg-white hover:border-blue-300 transition-all shadow-inner" 
                onClick={addNewStep}
              >
                <div className="w-20 h-20 rounded-full bg-white flex items-center justify-center text-slate-300 mb-6 shadow-sm border border-slate-100 group-hover:scale-110 group-hover:bg-blue-600 group-hover:text-white transition-all">
                   <svg className="w-10 h-10" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M12 4v16m8-8H4" /></svg>
                </div>
                <h4 className="text-xl font-black text-slate-500 mb-2 tracking-tight">Inga moment definierade</h4>
                <p className="text-slate-400 font-bold max-w-sm">Klicka här för att lägga till det första steget i denna rutin.</p>
              </div>
            )}
          </div>
        </section>

        {/* Action Bar */}
        <footer className="flex flex-col sm:flex-row justify-end items-center gap-6 pt-12 border-t-2 border-slate-200">
          <p className="text-xs text-slate-400 font-bold italic mr-auto hidden md:block">
            Alla ändringar granskas och valideras innan de blir tillgängliga i systemet.
          </p>
          <button
            type="button"
            onClick={() => navigate(-1)}
            className="px-12 py-5 text-slate-500 font-black hover:text-slate-900 transition-all text-lg"
          >
            Avbryt
          </button>
          <button
            type="submit"
            disabled={createMutation.isPending || updateMutation.isPending}
            className="w-full sm:w-auto px-20 py-5 bg-slate-900 text-white font-black rounded-[2.5rem] hover:bg-blue-600 shadow-2xl transition-all active:scale-95 disabled:opacity-50 text-lg flex items-center justify-center gap-4"
          >
            {(createMutation.isPending || updateMutation.isPending) ? (
              <>
                <div className="w-6 h-6 border-4 border-white border-t-transparent rounded-full animate-spin"></div>
                Sparar...
              </>
            ) : (
              <>
                <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M5 13l4 4L19 7" /></svg>
                {numericId ? 'Spara ändringar' : 'Skapa Rutin'}
              </>
            )}
          </button>
        </footer>
      </form>
    </div>
  );
};

export default Management;
