import { LogOut } from 'lucide-react';
import { Button, Separator, PageLoading } from '@/components/ui';
import { useAuth } from '@/api/auth';
import { MainErrorFallback } from '@/components/errors';
import { NavigationSection } from '@/components';
import { useIsMobile } from '@/hooks';
import { AccountSection } from '@/components/navigation/side-bar/account-section';
import { AdminSection } from '@/components/navigation/side-bar/admin-section';
import { LOGO_SHORT } from '@/assets';
import { Logo } from '@/components/logo';
import { useSideBar } from '@/stores';
import { useEffect } from 'react';
import { useLocation } from '@tanstack/react-router';

export type Item = {
  name:
    | 'Home'
    | 'Edit Store'
    | 'Meetings'
    // | 'Search'
    // | 'Notifications'
    // | 'Settings'
    | 'Integrations'
    | 'Payout'
    | 'Statistics'
    | 'Payouts Requests'
    | 'Orders';

  icon: JSX.Element;
  click: () => void;
  badge?: string;
};

const Sidebar = () => {
  const location = useLocation();
  const { sidebarOpen, setSidebarOpen, itemActive, setItemActive } = useSideBar();
  const { currentUser, error, isLoading, logout } = useAuth();
  const isMobile = useIsMobile();
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

  useEffect(() => {
    const path = location.pathname;
    switch (true) {
      case path.includes('store'):
        setItemActive('Edit Store');
        break;
      case path.includes('meets'):
        setItemActive('Meetings');
        break;
      case path.includes('statistics'):
        setItemActive('Statistics');
        break;
      case path.includes('payouts-requests'):
        setItemActive('Payouts Requests');
        break;
      case path.includes('integration'):
        setItemActive('Integrations');
        break;
      case path.includes('payout'):
        setItemActive('Payout');
        break;
      case path.includes('orders'):
        setItemActive('Orders');
        break;
      default:
        setItemActive('Home');
    }
  }, [location.pathname]);

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
