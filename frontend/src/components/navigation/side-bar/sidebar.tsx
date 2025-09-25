import { useAppNavigation } from '@/hooks/use-navigation';
import { X, LogOut, ChevronRight, ChevronLeft } from 'lucide-react';
import { LazyImage } from '@/utils/lazy-image';
import { Button, Separator, PageLoading } from '@/components/ui';
import { useAuth } from '@/api/auth';
import { MainErrorFallback } from '@/components/errors';
import { useSideBar } from '@/components';
import { useIsMobile } from '@/hooks';
import { AccountSection } from '@/components/navigation/side-bar/account-section';
import { NavigationSection } from '@/components/navigation/side-bar/navigation-section';
import { AdminSection } from '@/components/navigation/side-bar/admin-section';

export type Item = {
  name:
    | 'Home'
    | 'Edit Store'
    | 'Meetings'
    | 'Search'
    | 'Notifications'
    | 'Settings'
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

const Sidebar = ({ sidebarOpen, setSidebarOpen, collapsed = false, setCollapsed }: SidebarProps) => {
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
      {sidebarOpen && <div className="fixed inset-0 z-40 bg-black/50 lg:hidden" onClick={() => setSidebarOpen(false)} />}

      <aside
        className={` ${sidebarOpen ? 'translate-x-0' : '-translate-x-full'} fixed inset-y-0 left-0 z-50 lg:static lg:z-auto lg:translate-x-0 ${collapsed ? 'w-16' : 'w-80'} border-border bg-card flex h-full transform flex-col border-r shadow-xl transition-all duration-300 ease-in-out lg:h-screen lg:transform-none lg:shadow-none`}
      >
        <div
          className={`border-border flex items-center border-b ${collapsed ? 'flex h-14 justify-center p-2 shadow-2xl' : 'flex justify-between p-6'}`}
        >
          {collapsed ? (
            <LazyImage
              className="ring-primary/20 h-8 w-8 rounded-full object-cover ring-2"
              src={currentUser?.profilePicture.profilePictureLink || 'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'}
              alt="profile-pic"
              placeholder={
                currentUser?.profilePicture.thumbnailUrlPictureLink || 'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
              }
            />
          ) : (
            <>
              <div className="flex items-center gap-4">
                <LazyImage
                  className="ring-primary/20 h-auto w-12 rounded-full object-cover ring-2"
                  src={currentUser?.profilePicture.profilePictureLink || 'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'}
                  alt="profile-pic"
                  placeholder={
                    currentUser?.profilePicture.thumbnailUrlPictureLink || 'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
                  }
                />
                <div className="min-w-0 flex-1">
                  <div className="text-foreground truncate font-semibold">
                    {currentUser?.firstName} {currentUser?.lastName}
                  </div>
                  <button
                    onClick={() => nav.goToProfile(currentUser?.slug || '')}
                    className="text-accent-foreground hover:text-primary text-sm transition-colors"
                  >
                    View profile
                  </button>
                </div>
              </div>
              <Button variant="ghost" size="sm" onClick={() => setSidebarOpen(false)} className="hover:bg-muted h-8 w-8 cursor-pointer p-0 lg:hidden">
                <X className="h-4 w-4" />
              </Button>
            </>
          )}
        </div>

        {/* Navigation Section */}
        <div className={`flex-1 overflow-y-auto ${collapsed ? 'p-2' : 'p-4'}`}>
          <NavigationSection currentUser={currentUser} itemActive={itemActive} collapsed={collapsed} handleItemClick={handleItemClick} />

          {!collapsed && <Separator className="my-4" />}

          {/* Account Section */}
          <AccountSection itemActive={itemActive} handleItemClick={handleItemClick} collapsed={collapsed} />

          {currentUser?.roles?.includes('Admin') && <AdminSection itemActive={itemActive} handleItemClick={handleItemClick} collapsed={collapsed} />}

          {!collapsed && <Separator className="my-4" />}

          {/* Logout Button */}
          <Button
            variant="ghost"
            onClick={() => logout()}
            className={`text-destructive hover:text-destructive hover:bg-destructive/10 h-auto w-full ${collapsed ? 'justify-center p-2' : 'justify-start gap-3 px-3 py-2.5'} `}
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
            <div className="mt-5 flex items-center justify-start">
              <Button
                variant="outline"
                size="sm"
                onClick={() => setCollapsed(!collapsed)}
                className={`${collapsed ? 'h-6' : 'h-8'} bg-background w-full rounded-full border shadow-md hover:shadow-lg`}
              >
                {collapsed ? <ChevronRight className="h-3 w-3" /> : <ChevronLeft className="h-3 w-3" />}
              </Button>
            </div>
          )}
        </div>
      </aside>
    </>
  );
};

export default Sidebar;
