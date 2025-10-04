import { useAppNavigation } from '@/hooks/use-navigation';
import { X, LogOut, ChevronRight, ChevronLeft } from 'lucide-react';
import { LazyImage } from '@/lib/lazy-image';
import {
  Button,
  Separator,
  Sidebar as SidebarShadcn,
  PageLoading,
  SidebarContent,
  SidebarGroup,
  SidebarGroupLabel,
  SidebarGroupContent,
} from '@/components/ui';
import { useAuth } from '@/api/auth';
import { MainErrorFallback } from '@/components/errors';
import { NavigationSection, useSideBar } from '@/components';
import { useIsMobile } from '@/hooks';
import { AccountSection } from '@/components/navigation/side-bar/account-section';
import { AdminSection } from '@/components/navigation/side-bar/admin-section';
import { LOGO_SHORT } from '@/assets';
import { Logo } from '@/components/logo';

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

const Sidebar = () => {
  const { sidebarOpen, setSidebarOpen, toggleSidebar } = useSideBar();
  const { currentUser, error, isLoading, logout } = useAuth();
  const { itemActive, setItemActive } = useSideBar();
  const isMobile = useIsMobile();
  const nav = useAppNavigation();
  const collapsed = !sidebarOpen;

  if (isLoading) return <PageLoading />;
  if (error || !currentUser) return <MainErrorFallback />;

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
        className={` ${sidebarOpen ? 'translate-x-0' : '-translate-x-full'} fixed inset-y-0 left-0 z-50 lg:static lg:z-auto lg:translate-x-0 ${collapsed ? 'w-16' : 'w-80'} border-border/40 bg-card flex h-full flex-col border-r shadow-xl transition-all duration-300 ease-in-out lg:h-screen lg:shadow-none`}
        style={{
          transitionProperty: 'transform, width',
        }}
      >
        {/* Navigation Section */}
        <div className={`flex-1 overflow-y-auto ${collapsed ? 'p-2' : 'p-4'}`}>
          {collapsed ? <img src={LOGO_SHORT} className={`mx-auto mb-6 h-12 w-auto`} /> : <Logo className="mb-6 pl-2" />}

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
        </div>
      </aside>
    </>
  );
};

export default Sidebar;
