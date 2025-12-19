
import React from 'react';

interface IconProps {
  iconKey: string;
  className?: string;
}

const Icon: React.FC<IconProps> = ({ iconKey, className = "w-6 h-6" }) => {
  const normalizedKey = iconKey.toLowerCase();

  switch (normalizedKey) {
    case 'wash':
    case 'hygiene':
      return <svg className={className} fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M14.828 14.828a4 4 0 01-5.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>;
    case 'medicine':
    case 'pill':
      return <svg className={className} fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L5.594 15.12a2 2 0 01-1.183-2.281l.888-4.442a2 2 0 011.183-1.516l4.442-.888a2 2 0 011.516.335l2.221 1.11a2 2 0 01.335 3.033l-1.11 2.221a2 2 0 00.335 2.221l2.221 1.11a2 2 0 01.335 3.033z" /></svg>;
    case 'clean':
    case 'spray':
      return <svg className={className} fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19.428 15.428a2 2 0 00-1.022-.547l-2.387-.477a6 6 0 00-3.86.517l-.318.158a6 6 0 01-3.86.517L5.594 15.12a2 2 0 01-1.183-2.281l.888-4.442a2 2 0 011.183-1.516l4.442-.888a2 2 0 011.516.335l2.221 1.11a2 2 0 01.335 3.033l-1.11 2.221a2 2 0 00.335 2.221l2.221 1.11a2 2 0 01.335 3.033z" /></svg>;
    case 'fasting':
    case 'no-food':
      return <svg className={className} fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M18.364 18.364A9 9 0 005.636 5.636m12.728 12.728A9 9 0 015.636 5.636m12.728 12.728L5.636 5.636" /></svg>;
    case 'remove':
    case 'trash':
      return <svg className={className} fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>;
    default:
      return <svg className={className} fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" /></svg>;
  }
};

export default Icon;
