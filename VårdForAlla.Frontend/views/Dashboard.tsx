
import React from 'react';
import { Link } from 'react-router-dom';
import { useRoutines } from '../hooks/useRoutines';
import { useAuth } from '../context/AuthContext';
import RoutineCard from '../components/RoutineCard';

const Dashboard: React.FC = () => {
  const { user } = useAuth();
  const isAdmin = user?.role === 'Admin';

  const { data: allRoutines, isLoading: isLoadingAll } = useRoutines(false);
  const { data: templates, isLoading: isLoadingTemplates } = useRoutines(true);

  const quickLinks = [
    { label: 'Mina Rutiner', path: '/routines', color: 'bg-blue-600', icon: 'M5 19a2 2 0 01-2-2V7a2 2 0 012-2h4l2 2h4a2 2 0 012 2v1M5 19h14a2 2 0 002-2v-5a2 2 0 00-2-2H9l-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z', adminOnly: false },
    { label: 'Alla Mallar', path: '/routines?type=template', color: 'bg-purple-600', icon: 'M8 7v8a2 2 0 002 2h6M8 7V5a2 2 0 012-2h4.586a1 1 0 01.707.293l4.414 4.414a1 1 0 01.293.707V15a2 2 0 01-2 2h-2M8 7H6a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2v-2', adminOnly: false },
    { label: 'Administration', path: '/manage', color: 'bg-slate-600', icon: 'M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z', adminOnly: true },
  ];

  return (
    <div className="space-y-12">
      {/* Hero Section */}
      <section className="bg-gradient-to-br from-blue-700 to-indigo-800 rounded-[3rem] p-10 md:p-16 text-white shadow-2xl relative overflow-hidden">
        <div className="relative z-10 max-w-2xl">
          <h2 className="text-4xl md:text-5xl font-black mb-6 leading-tight tracking-tight">Välkommen till VårdFörAlla</h2>
          <p className="text-lg md:text-xl text-blue-100 font-medium mb-10 leading-relaxed">
            Hantera medicinska rutiner och patientkommunikation på ett enkelt och strukturerat sätt för säkrare vård.
          </p>
          <div className="flex flex-wrap gap-4">
            <Link 
              to="/manage" 
              className="px-8 py-4 bg-white text-blue-700 font-black rounded-2xl hover:bg-blue-50 transition-all shadow-lg active:scale-95"
            >
              Skapa Ny Rutin
            </Link>
            <Link 
              to="/routines" 
              className="px-8 py-4 bg-blue-600/30 text-white font-black rounded-2xl border border-blue-400/50 hover:bg-blue-600/50 transition-all active:scale-95 backdrop-blur-sm"
            >
              Visa Alla Rutiner
            </Link>
          </div>
        </div>
        
        {/* Abstract shapes */}
        <div className="absolute top-0 right-0 w-96 h-96 bg-white/10 rounded-full -mr-32 -mt-32 blur-3xl"></div>
        <div className="absolute bottom-0 right-0 w-64 h-64 bg-indigo-500/20 rounded-full -mb-20 -mr-20 blur-2xl"></div>
      </section>

      {/* Quick Links Grid */}
      <section className="grid grid-cols-1 sm:grid-cols-3 gap-6">
        {quickLinks.map((link) => (
          <Link 
            key={link.label} 
            to={link.path}
            className={`group bg-white p-6 rounded-3xl border border-slate-100 shadow-sm hover:shadow-xl hover:border-blue-200 transition-all duration-300 ${(!isAdmin && link.adminOnly) ? 'opacity-50 grayscale cursor-not-allowed' : ''}`}
          >
            <div className={`${link.color} w-12 h-12 rounded-2xl flex items-center justify-center text-white mb-4 group-hover:scale-110 transition-transform`}>
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d={link.icon} />
              </svg>
            </div>
            <h3 className="text-xl font-black text-slate-800 tracking-tight">{link.label}</h3>
            <p className="text-slate-400 text-sm font-medium mt-1 group-hover:text-blue-500 transition-colors">Snabbåtkomst & hantering →</p>
          </Link>
        ))}
      </section>

      {/* Featured / Templates Section */}
      <section className="space-y-6">
        <div className="flex items-center justify-between">
          <h3 className="text-2xl font-black text-slate-900 tracking-tight flex items-center gap-3">
            <span className="w-2 h-8 bg-purple-600 rounded-full"></span>
            Populära Mallar
          </h3>
          <Link to="/routines?type=template" className="text-sm font-bold text-blue-600 hover:text-blue-700 px-4 py-2 rounded-xl bg-blue-50">Visa alla mallar</Link>
        </div>
        
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {isLoadingTemplates ? (
             [1, 2].map(i => <div key={i} className="h-48 bg-white border border-slate-100 rounded-3xl animate-pulse"></div>)
          ) : !templates || (Array.isArray(templates) ? templates.length === 0 : templates.items?.length === 0) ? (
            <div className="col-span-full py-10 bg-slate-50 rounded-3xl text-center border-2 border-dashed border-slate-200">
              <p className="text-slate-400 font-medium">Inga mallar tillgängliga just nu.</p>
            </div>
          ) : (
            (Array.isArray(templates) ? templates : templates.items || []).slice(0, 2).map(routine => <RoutineCard key={routine.id} routine={routine} />)
          )}
        </div>
      </section>

      {/* Recent Routines Section */}
      <section className="space-y-6">
        <div className="flex items-center justify-between">
          <h3 className="text-2xl font-black text-slate-900 tracking-tight flex items-center gap-3">
            <span className="w-2 h-8 bg-blue-600 rounded-full"></span>
            Senast Uppdaterade
          </h3>
          <Link to="/routines" className="text-sm font-bold text-blue-600 hover:text-blue-700 px-4 py-2 rounded-xl bg-blue-50">Hantera alla</Link>
        </div>
        
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {isLoadingAll ? (
             [1, 2, 3].map(i => <div key={i} className="h-48 bg-white border border-slate-100 rounded-3xl animate-pulse"></div>)
          ) : !allRoutines || (Array.isArray(allRoutines) ? allRoutines.length === 0 : allRoutines.items?.length === 0) ? (
            <div className="col-span-full py-10 bg-slate-50 rounded-3xl text-center border-2 border-dashed border-slate-200">
              <p className="text-slate-400 font-medium">Inga rutiner publicerade än.</p>
            </div>
          ) : (
            (Array.isArray(allRoutines) ? allRoutines : allRoutines.items || []).slice(0, 3).map(routine => <RoutineCard key={routine.id} routine={routine} />)
          )}
        </div>
      </section>
    </div>
  );
};

export default Dashboard;