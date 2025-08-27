import { Logo } from '@/components/logo';
import { Badge, Button, LoadingState } from '@/components/ui';
import { useAuth } from '@/features/auth';
import { useGlobalErrorStore, useSideBar } from '@/stores';
import { LazyImage } from '@/utils';
import { Bell, CreditCard, Menu } from 'lucide-react';
import { useAppNavigation } from '@/hooks';
import { api } from '@/lib';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import {
  queryOptions,
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import { ErrorComponenet, GlobalErrorHandling } from '@/components/errors';

type Wallet = {
  balance: number;
};

async function getWallet(): Promise<Wallet> {
  const res = await api.get<Wallet>(MentorshipEndpoints.Payments.Wallet);
  return res;
}

function useGetWallet(
  overrides?: Partial<UseQueryOptions<Wallet, Error>>,
): UseQueryResult<Wallet, Error> {
  return useQuery<Wallet, Error>(
    queryOptions({
      queryFn: getWallet,
      queryKey: ['wallet'], // todo fix this
      ...overrides,
    }),
  );
}

function BalanceHeader() {
  const { data: walletData, isLoading } = useGetWallet();

  if (isLoading) {
    return <LoadingState type="dots"></LoadingState>;
  }

  return (
    <Button className="bg-yellow-50 text-primary pl-5 pr-2 hover:bg-yellow-100/80 hidden md:inline-flex   ">
      <div className="space-x-1">
        <span className="font-semibold"> Balance </span>
        <span className="font-bold">{`${walletData?.balance}$`}</span>
      </div>
      <div className="bg-yellow-300 ml-auto p-1 rounded-md ">
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
    <header className="sticky top-0 z-30 bg-card border-b border-border shadow-sm">
      <div className="flex items-center justify-between px-4 py-3">
        {/* Left side - Logo and Menu (Mobile only) */}
        <div className="flex items-center gap-3">
          <Button
            variant="ghost"
            size="sm"
            onClick={() => setSidebarOpen(true)}
            className="h-7.5 w-10 p-0 hover:bg-muted "
          >
            <Menu className="w-5 h-5" />
          </Button>
          <Logo className="h-8" />
        </div>

        {/* Right side - Profile and Notifications */}
        {
          <div className="flex h-7.5 items-center gap-2 ">
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

            <Button
              variant="ghost"
              size="sm"
              onClick={() => navigate.goToProfile(currentUser.slug)}
              className="h-10 px-2 hover:bg-muted"
            >
              <LazyImage
                className="w-7 h-7 rounded-full ring-2 ring-primary/20 object-cover"
                src={
                  currentUser?.profilePicture.profilePictureLink ||
                  'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
                }
                alt="profile-pic"
                placeholder={
                  currentUser?.profilePicture.thumbnailUrlPictureLink ||
                  'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
                }
              />
            </Button>
          </div>
        }
      </div>
    </header>
  );
}
