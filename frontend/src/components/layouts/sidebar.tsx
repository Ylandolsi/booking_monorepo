import { useNavigate } from '@tanstack/react-router';
import {
  X,
  Settings,
  LogOut,
  MessageCircleQuestion,
  Bell,
  Home,
  User as UserIcon,
  Calendar,
  Search,
  ChevronRight,
} from 'lucide-react';
import { LazyImage } from '@/utils/lazy-image';

import { Link, Button, Badge, Separator } from '@/components/ui';
import {} from '../ui/separator';
import { useAuth } from '@/features/auth/hooks';
import { MainErrorFallback } from '../errors/main';
import { Spinner } from '../ui';

type SidebarProps = {
  sidebarOpen: boolean;
  setSidebarOpen: (open: boolean) => void;
};

const Sidebar = ({ sidebarOpen, setSidebarOpen }: SidebarProps) => {
  const { user, error, isLoading, logout } = useAuth();
  const navigate = useNavigate();
  if (error) return <MainErrorFallback />;
  if (isLoading) return <Spinner />;

  const navigationItems = [
    {
      name: 'Home',
      icon: <Home size={20} />,
      click: () => navigate({ to: '/app' }),
      active: true,
    },
    {
      name: 'Profile',
      icon: <UserIcon size={20} />,
      click: () => navigate({ to: `/members/${user?.id}` }),
      active: false,
    },
    {
      name: 'Bookings',
      icon: <Calendar size={20} />,
      click: () => {},
      active: false,
      badge: '3',
    },
    {
      name: 'Search',
      icon: <Search size={20} />,
      click: () => {},
      active: false,
    },
  ];

  const accountItems = [
    {
      name: 'Notifications',
      icon: <Bell size={20} />,
      click: () => {},
      badge: '5',
    },
    {
      name: 'Settings',
      icon: <Settings size={20} />,
      click: () => {},
    },
    {
      name: 'Support',
      icon: <MessageCircleQuestion size={20} />,
      click: () => {},
    },
  ];

  return (
    <>
      {/* Background overlay */}
      {sidebarOpen && (
        <div
          className="fixed inset-0 bg-black/50 z-40 lg:hidden"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      {/* Sidebar */}
      <div
        className={`fixed top-0 left-0 h-full w-80 bg-card border-r border-border shadow-xl transform transition-transform duration-300 ease-in-out z-50 overflow-y-auto ${
          sidebarOpen ? 'translate-x-0' : '-translate-x-full'
        }`}
      >
        {/* User Profile Section */}
        <div className="p-6 border-b border-border justify-between flex">
          <div className="flex items-center gap-4">
            <LazyImage
              className="w-12 h-auto rounded-full ring-2 ring-primary/20 object-cover"
              src={
                user?.profilePictureUrl ||
                'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
              }
              alt="profile-pic"
              placeholder={
                user?.profilePictureUrl ||
                'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
              }
            />
            <div className="flex-1 min-w-0">
              <div className="font-semibold text-foreground truncate">
                {user?.firstName} {user?.lastName}
              </div>
              <Link
                to={`/members/${user?.id}`}
                className="text-sm text-muted-foreground hover:text-primary transition-colors"
              >
                View profile
              </Link>
            </div>
          </div>
          <Button
            variant="ghost"
            size="sm"
            onClick={() => setSidebarOpen(false)}
            className="h-8 w-8 p-0 hover:bg-muted cursor-pointer"
          >
            <X className="w-4 h-4" />
          </Button>
        </div>

        {/* Navigation Section */}
        <div className="p-4">
          <div className="mb-6">
            <h3 className="px-2 mb-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
              Navigation
            </h3>
            <nav className="space-y-1">
              {navigationItems.map((item) => (
                <button
                  key={item.name}
                  onClick={() => {
                    item.click();
                    setSidebarOpen(false);
                  }}
                  className={`w-full flex items-center gap-3 px-3 py-2.5 rounded-lg text-left transition-all duration-200 group ${
                    item.active
                      ? 'bg-primary text-primary-foreground shadow-md'
                      : 'hover:bg-muted text-muted-foreground hover:text-foreground'
                  }`}
                >
                  <span
                    className={`${item.active ? 'text-primary-foreground' : 'text-muted-foreground group-hover:text-foreground'}`}
                  >
                    {item.icon}
                  </span>
                  <span className="font-medium flex-1">{item.name}</span>
                  {item.badge && (
                    <Badge variant="secondary" className="h-5 px-2 text-xs">
                      {item.badge}
                    </Badge>
                  )}
                  <ChevronRight
                    className={`w-4 h-4 transition-transform ${item.active ? 'text-primary-foreground' : 'text-muted-foreground group-hover:text-foreground'}`}
                  />
                </button>
              ))}
            </nav>
          </div>

          <Separator className="my-4" />

          {/* Account Section */}
          <div className="mb-6">
            <h3 className="px-2 mb-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
              Account
            </h3>
            <nav className="space-y-1">
              {accountItems.map((item) => (
                <button
                  key={item.name}
                  onClick={() => {
                    item.click();
                    setSidebarOpen(false);
                  }}
                  className="w-full flex items-center gap-3 px-3 py-2.5 rounded-lg text-left transition-all duration-200 group hover:bg-muted text-muted-foreground hover:text-foreground"
                >
                  <span className="text-muted-foreground group-hover:text-foreground">
                    {item.icon}
                  </span>
                  <span className="font-medium flex-1">{item.name}</span>
                  {item.badge && (
                    <Badge variant="destructive" className="h-5 px-2 text-xs">
                      {item.badge}
                    </Badge>
                  )}
                  <ChevronRight className="w-4 h-4 text-muted-foreground group-hover:text-foreground" />
                </button>
              ))}
            </nav>
          </div>

          <Separator className="my-4" />

          {/* Logout Button */}
          <Button
            variant="ghost"
            onClick={logout}
            className="w-full justify-start gap-3 text-destructive hover:text-destructive hover:bg-destructive/10 px-3 py-2.5 h-auto"
          >
            <div
              className="flex gap-2
            "
            >
              <LogOut size={20} />
              <span className="font-medium">Logout</span>
            </div>
          </Button>
        </div>
      </div>
    </>
  );
};

export default Sidebar;
