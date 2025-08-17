import { MobileHeader, DesktopHeader } from '@/features/app/profile';
import { useBreakpoint } from '@/hooks';

export function Header() {
  const { isMobile } = useBreakpoint();
  return <>{isMobile ? <MobileHeader /> : <DesktopHeader />}</>;
}
