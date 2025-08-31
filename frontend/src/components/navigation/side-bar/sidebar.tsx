import { useAppNavigation } from '@/hooks/use-navigation';
import { X, LogOut, ChevronRight, ChevronLeft } from 'lucide-react';
import { LazyImage } from '@/utils/lazy-image';
import { Button, Separator, PageLoading } from '@/components/ui';
import { useAuth } from '@/features/auth/hooks';
import { MainErrorFallback } from '@/components/errors';
import { useSideBar } from '@/components';
import { useIsMobile } from '@/hooks';
import { AccountSection } from '@/components/navigation/side-bar/account-section';
import { NavigationSection } from '@/components/navigation/side-bar/navigation-section';
import { AdminSection } from '@/components/navigation/side-bar/admin-section';

export type Item = {
  name:
    | 'Home'
    | 'Profile'
    | 'Meetings'
    | 'Search'
    | 'Notifications'
    | 'Settings'
    | 'Become Mentor'
    | 'Set Availability'
    | 'Integrations'
    | 'Payout'
    | 'Payouts Requests';

  icon: JSX.Element;
  click: () => void;
  badge?: string;
};

type SidebarProps = {
  sidebarOpen: boolean;
  setSidebarOpen: (open: boolean) => void;
  collapsed?: boolean;
  setCollapsed?: (collapsed: boolean) => void;
};

const Sidebar = ({
  sidebarOpen,
  setSidebarOpen,
  collapsed = false,
  setCollapsed,
}: SidebarProps) => {
  const { currentUser, error, isLoading, logout } = useAuth();
  const { itemActive, setItemActive } = useSideBar();
  const isMobile = useIsMobile();
  const nav = useAppNavigation();

  if (error) return <MainErrorFallback />;
  if (isLoading) return <PageLoading />;

  const handleItemClick = (item: Item) => {
    setItemActive(item.name);
    item.click();
    // Only close sidebar on mobile
    if (isMobile) {
      setSidebarOpen(false);
    }
  };

  return (
    <>
      {/* Mobile overlay */}
      {sidebarOpen && (
        <div
          className="fixed inset-0 bg-black/50 z-40 lg:hidden"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      <aside
        className={`
          ${sidebarOpen ? 'translate-x-0' : '-translate-x-full'}
          lg:translate-x-0
          fixed lg:static
          z-50 lg:z-auto
          inset-y-0 left-0
          ${collapsed ? 'w-16' : 'w-80'}
          border-r border-border
          bg-card 
          transform lg:transform-none transition-all duration-300 ease-in-out
          flex flex-col
          h-full lg:h-screen
          shadow-xl lg:shadow-none
        `}
      >
        <div
          className={`border-b border-border flex items-center ${collapsed ? 'p-2 flex justify-center h-14  shadow-2xl' : 'p-6 flex justify-between'}`}
        >
          {collapsed ? (
            <LazyImage
              className="w-8 h-8 rounded-full ring-2 ring-primary/20 object-cover"
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
          ) : (
            <>
              <div className="flex items-center gap-4">
                <LazyImage
                  className="w-12 h-auto rounded-full ring-2 ring-primary/20 object-cover"
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
                <div className="flex-1 min-w-0">
                  <div className="font-semibold text-foreground truncate">
                    {currentUser?.firstName} {currentUser?.lastName}
                  </div>
                  <button
                    onClick={() => nav.goToProfile(currentUser?.slug || '')}
                    className="text-sm text-muted-foreground hover:text-primary transition-colors"
                  >
                    View profile
                  </button>
                </div>
              </div>
              <Button
                variant="ghost"
                size="sm"
                onClick={() => setSidebarOpen(false)}
                className="h-8 w-8 p-0 hover:bg-muted cursor-pointer lg:hidden"
              >
                <X className="w-4 h-4" />
              </Button>
            </>
          )}
        </div>

        {/* Navigation Section */}
        <div className={`flex-1 overflow-y-auto ${collapsed ? 'p-2' : 'p-4'}`}>
          <NavigationSection
            currentUser={currentUser}
            itemActive={itemActive}
            collapsed={collapsed}
            handleItemClick={handleItemClick}
          />

          {!collapsed && <Separator className="my-4" />}

          {/* Account Section */}
          <AccountSection
            itemActive={itemActive}
            handleItemClick={handleItemClick}
            collapsed={collapsed}
          />

          {currentUser?.roles?.includes('Admin') && (
            <AdminSection
              itemActive={itemActive}
              handleItemClick={handleItemClick}
              collapsed={collapsed}
            />
          )}

          {!collapsed && <Separator className="my-4" />}

          {/* Logout Button */}
          <Button
            variant="ghost"
            onClick={() => logout()}
            className={`
              w-full text-destructive hover:text-destructive hover:bg-destructive/10 h-auto
              ${collapsed ? 'justify-center p-2' : 'justify-start gap-3 px-3 py-2.5'}
            `}
            title={collapsed ? 'Logout' : undefined}
          >
            {collapsed ? (
              <LogOut size={20} />
            ) : (
              <div className="flex gap-2">
                <LogOut size={20} />
                <span className="font-medium">Logout</span>
              </div>
            )}
          </Button>

          {setCollapsed && (
            <div className="flex mt-5 justify-start items-center">
              <Button
                variant="outline"
                size="sm"
                onClick={() => setCollapsed(!collapsed)}
                className={`${collapsed ? 'h-6' : 'h-8'} w-full rounded-full bg-background border shadow-md hover:shadow-lg`}
              >
                {collapsed ? (
                  <ChevronRight className="w-3 h-3" />
                ) : (
                  <ChevronLeft className="w-3 h-3" />
                )}
              </Button>
            </div>
          )}
        </div>
      </aside>
    </>
  );
};

export default Sidebar;
