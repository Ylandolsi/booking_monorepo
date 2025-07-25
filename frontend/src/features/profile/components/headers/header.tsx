import { useBreakpoint } from '@/hooks/use-media-query';
import { MobileHeader } from './mobile-header';
import { DesktopHeader } from './desktop-header';

export function Header() {
  const { isMobile } = useBreakpoint();
  return (
    <>
      {isMobile ? (
        <MobileHeader/>
      ) : (
        <DesktopHeader/>
      )}
    </>
  );
}
