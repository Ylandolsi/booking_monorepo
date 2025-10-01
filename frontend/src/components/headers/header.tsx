import { Logo } from '@/components/logo';
import { Button, LoadingState } from '@/components/ui';
import { useAuth } from '@/api/auth';
import { useSideBar } from '@/stores';
import { CreditCard, Menu, Sidebar } from 'lucide-react';
import { ThemeToggle } from '@/components/theme-toggle';
import { useGetWallet } from '@/api/stores';

function BalanceHeader() {
  const { data: walletData, isLoading } = useGetWallet();

  if (isLoading) {
    return <LoadingState type="dots"></LoadingState>;
  }

  return (
    <Button className="bg-accent -primary text-primary hover:bg-primary-100/80 hidden border pr-2 pl-5 md:inline-flex">
      <div className="space-x-1">
        <span className="font-semibold"> Balance </span>
        <span className="font-bold">{`${walletData?.balance}$`}</span>
      </div>
      <div className="bg-primary-300 ml-auto rounded-md p-1">
        <CreditCard className="text-primary" />
      </div>
    </Button>
  );
}

export function Header() {
  const { toggleSidebar } = useSideBar();
  const { currentUser } = useAuth();

  if (!currentUser || currentUser == undefined) return null;

  return (
    <header className="bg-card border-border sticky top-0 z-30 border-b shadow-sm">
      <div className="flex items-center justify-between px-4 py-3">
        {/* Left side - Logo and Menu (Mobile only) */}
        <div className="flex items-center gap-3">
          <Button variant="ghost" size="sm" onClick={toggleSidebar} className="hover:bg-muted h-7.5 w-10 p-0">
            <Sidebar className="h-5 w-5" />
          </Button>
          <Logo className="h-8" />
        </div>

        {
          <div className="flex h-7.5 items-center gap-2">
            <BalanceHeader />
            <ThemeToggle />
            {/* // TODO change this with link of store preview */}
          </div>
        }
      </div>
    </header>
  );
}
