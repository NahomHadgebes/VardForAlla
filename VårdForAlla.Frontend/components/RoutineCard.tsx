import React from 'react';
import { Link } from 'react-router-dom';
import { RoutineListDto } from '../types';
import Icon from './Icon';

interface RoutineCardProps {
  routine: RoutineListDto;
}

const RoutineCard: React.FC<RoutineCardProps> = ({ routine }) => {
  // Säkerhetskontroll för stepCount med fallback
  const stepCount = routine.stepCount ?? 0;
  const tags = routine.tags ?? [];
  const description = routine.description ?? "Ingen sammanfattning tillgänglig för denna rutin.";

  return (
    <Link
      to={`/routine/${routine.id}`}
      className="bg-white p-8 rounded-[2.5rem] border-2 border-slate-100 shadow-xl shadow-slate-100/50 hover:shadow-2xl hover:border-slate-900 transition-all duration-300 group flex flex-col h-full relative overflow-hidden"
    >
      <div className="absolute top-0 right-0 w-24 h-24 bg-slate-50 rounded-bl-full -mr-8 -mt-8 group-hover:bg-slate-100 transition-colors"></div>
      
      <div className="relative z-10 flex justify-between items-start mb-6">
        <span className="inline-block px-4 py-1.5 text-[10px] font-black text-slate-900 bg-slate-50 rounded-xl uppercase tracking-widest border border-slate-200 group-hover:bg-slate-900 group-hover:text-white transition-all">
          {routine.category}
        </span>
        <div className="w-10 h-10 rounded-xl bg-slate-50 flex items-center justify-center text-slate-400 group-hover:bg-slate-900 group-hover:text-white transition-all shadow-sm">
          <svg className="w-5 h-5 group-hover:translate-x-0.5 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="3" d="M9 5l7 7-7 7" />
          </svg>
        </div>
      </div>
      
      <div className="relative z-10 mb-2">
        <h3 className="text-2xl font-black text-slate-900 leading-tight tracking-tight group-hover:text-blue-600 transition-colors">
          {routine.title}
        </h3>
      </div>
      
      <p className="relative z-10 text-sm text-slate-500 font-bold line-clamp-2 mb-8 flex-grow leading-relaxed italic">
        {description}
      </p>
      
      <div className="relative z-10 flex flex-wrap gap-2 mb-8">
        {tags.slice(0, 3).map((tag) => (
          <span key={tag} className="text-[9px] font-black bg-slate-100 text-slate-900 px-3 py-1 rounded-lg uppercase tracking-wider border border-slate-200">
            #{tag}
          </span>
        ))}
        {tags.length > 3 && (
          <span className="text-[9px] font-black text-slate-400 px-1 py-1 uppercase">
            +{tags.length - 3} mer
          </span>
        )}
      </div>
      
      <div className="relative z-10 mt-auto pt-6 border-t-2 border-slate-50 flex items-center justify-between text-[11px] text-slate-900 font-black uppercase tracking-widest">
        <div className="flex items-center gap-2">
           <div className="w-6 h-6 rounded-lg bg-slate-50 flex items-center justify-center border border-slate-100">
              <Icon iconKey="default" className="w-3 h-3 text-slate-900" />
           </div>
           <span>{stepCount} Moment</span>
        </div>
        <span className="text-slate-400">
          {routine.updatedAt ? new Date(routine.updatedAt).toLocaleDateString('sv-SE') : ''}
        </span>
      </div>
    </Link>
  );
};

export default RoutineCard;