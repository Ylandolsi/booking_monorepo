import { cn } from '@/lib';
import { IPhoneMockup } from 'react-device-mockup';

interface MobileContainerProps {
  children: React.ReactNode;
  className?: string;
  screenWidth?: number;
}

export function MobileContainer({ screenWidth, children }: MobileContainerProps) {
  return (
    <IPhoneMockup screenWidth={screenWidth || 400}>
      <div className={cn('bg-background flex h-full w-full flex-col items-center space-y-4 overflow-y-auto p-4 text-center', children)}>
        {children}
      </div>
    </IPhoneMockup>
  );
}
