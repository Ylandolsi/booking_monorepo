import { Logo } from '@/components/logo';
import { Button, LoadingState } from '@/components/ui';
import { useAuth } from '@/features/auth';
import { useSideBar } from '@/stores';
import { LazyImage } from '@/utils';
import { CreditCard, Menu } from 'lucide-react';
import { useAppNavigation } from '@/hooks';
import { useGetWallet } from '@/features/shared/get-wallet';
import { FALLBACK_PROFILE_PICTURE } from '@/lib';

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
  const { setSidebarOpen } = useSideBar();
  const { currentUser } = useAuth();
  const navigate = useAppNavigation();

  if (!currentUser || currentUser == undefined) return null;

  return (
    <header className="bg-card border-border sticky top-0 z-30 border-b shadow-sm">
      <div className="flex items-center justify-between px-4 py-3">
        {/* Left side - Logo and Menu (Mobile only) */}
        <div className="flex items-center gap-3">
          <Button variant="ghost" size="sm" onClick={() => setSidebarOpen(true)} className="hover:bg-muted h-7.5 w-10 p-0">
            <Menu className="h-5 w-5" />
          </Button>
          <Logo className="h-8" />
        </div>

        {/* Right side - Profile and Notifications */}
        {
          <div className="flex h-7.5 items-center gap-2">
            {/* <Button
              variant="ghost"
              size="sm"
              className="h-10 w-10 p-0 hover:bg-muted relative"
            >
              <Bell className="w-5 h-5" />
              <Badge
                variant="destructive"
                className="absolute -top-1 -right-1 h-5 w-5 p-0 text-xs flex items-center justify-center"
              >
                3
              </Badge>
            </Button> */}
            <BalanceHeader />

            <Button variant="ghost" size="sm" onClick={() => navigate.goToProfile(currentUser.slug)} className="hover:bg-muted h-10 px-2">
              <LazyImage
                className="ring-primary/20 h-7 w-7 rounded-full object-cover ring-2"
                src={currentUser?.profilePicture.profilePictureLink || FALLBACK_PROFILE_PICTURE}
                alt="profile-pic"
                placeholder={currentUser?.profilePicture.thumbnailUrlPictureLink || FALLBACK_PROFILE_PICTURE}
              />
            </Button>
          </div>
        }
      </div>
    </header>
  );
}
