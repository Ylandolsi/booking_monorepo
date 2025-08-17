import { useBreakpoint } from '@/hooks';
import { MobileHeader, DesktopHeader } from '@/features/app/profile';

export function Header() {
  const { isMobile } = useBreakpoint();
  return <>{isMobile ? <MobileHeader /> : <DesktopHeader />}</>;
}
