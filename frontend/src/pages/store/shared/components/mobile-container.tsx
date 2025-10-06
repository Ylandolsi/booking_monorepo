import { IPHONE_MOCKUP } from '@/assets';
import { cn } from '@/lib';

interface MobileContainerProps {
  children: React.ReactNode;
  className?: string;
}

export function MobileContainer({ children, className }: MobileContainerProps) {
  return (
    <div className={cn('bg-background relative w-fit overflow-hidden rounded-4xl', className)}>
      <img src={IPHONE_MOCKUP} className="z-40"></img>

      <div
        // className="absolute inset-0 mx-auto mt-10 mb-10 w-[88%] overflow-x-hidden overflow-y-scroll rounded-2xl"

        className="absolute inset-0 mx-auto mt-10 mb-10 w-[90%] overflow-x-hidden overflow-y-scroll rounded-2xl"
        style={
          {
            scrollbarWidth: 'none',
            msOverflowStyle: 'none',
            maskImage: 'linear-gradient(to bottom, transparent 0%, black 5%, black 95%, transparent 100%)',
            WebkitMaskImage: 'linear-gradient(to bottom, transparent 0%, black 5%, black 95%, transparent 100%)',
          } as React.CSSProperties
        }
      >
        {/* <img src={STATUS_BAR} className="fixed inset-0 top-10"></img> */}

        {children}
      </div>
    </div>
  );
}
