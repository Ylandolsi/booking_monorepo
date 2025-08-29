import { useAppNavigation } from '@/hooks/use-navigation';
import {
  X,
  Settings,
  LogOut,
  Bell,
  Home,
  User as UserIcon,
  Calendar,
  Search,
  ChevronRight,
  ChevronLeft,
  Timer,
} from 'lucide-react';
import { LazyImage } from '@/utils/lazy-image';
import { Button, Badge, Separator, PageLoading } from '@/components/ui';
import { useAuth } from '@/features/auth/hooks';
import { MainErrorFallback } from '@/components/errors';
import { useSideBar } from '@/components';
import { useIsMobile } from '@/hooks';
import { GiTeacher } from 'react-icons/gi';
import { FaGoogle } from 'react-icons/fa';
import { googleOIDC } from '@/features/auth';

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
    | 'Integrate With Google'
    | 'Already Integrated';
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
  const navigationItems: Item[] = [
    {
      name: 'Home',
      icon: <Home size={20} />,
      click: () => nav.goToApp(),
    },
    {
      name: 'Profile',
      icon: <UserIcon size={20} />,
      click: () => nav.goToProfile(currentUser?.slug || ''),
    },
    {
      name: 'Become Mentor',
      icon: <GiTeacher size={20} />,
      click: () => nav.goToMentorBecome(),
    },
    {
      name: 'Set Availability',
      icon: <Timer size={20} />,
      click: () => nav.goToMentorSetSchedule(),
    },
    {
      name: 'Meetings',
      icon: <Calendar size={20} />,
      click: () => {
        nav.goToMeets();
      },
      badge: '3',
    },
    {
      name: 'Search',
      icon: <Search size={20} />,
      click: () => {},
    },
  ];

  const accountItems: Item[] = [
    {
      icon: <FaGoogle size={20} />,
      click: async () => await googleOIDC(),
      name: 'Integrate With Google',
    },
    // {
    //   name: 'Notifications',
    //   icon: <Bell size={20} />,
    //   click: () => {},
    //   badge: '5',
    // },
    // {
    //   name: 'Settings',
    //   icon: <Settings size={20} />,
    //   click: () => {},
    // },
  ];
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
          <div className="mb-6">
            {!collapsed && (
              <h3 className="px-2 mb-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
                Navigation
              </h3>
            )}
            <nav className="space-y-1">
              {navigationItems.map((item) => (
                <Button
                  variant={'ghost'}
                  key={item.name}
                  onClick={() => handleItemClick(item)}
                  className={`
                    w-full flex items-center rounded-lg text-left transition-all duration-200 group relative
                    ${collapsed ? 'justify-center p-2' : 'gap-3 px-3 py-2.5'}
                    ${
                      itemActive == item.name
                        ? 'bg-primary text-primary-foreground shadow-md'
                        : 'hover:bg-accent/20 text-muted-foreground hover:text-foreground'
                    }
                  `}
                  title={collapsed ? item.name : undefined}
                >
                  <span
                    className={`${itemActive == item.name ? 'text-primary-foreground' : 'text-muted-foreground group-hover:text-foreground'}`}
                  >
                    {item.icon}
                  </span>
                  {!collapsed && (
                    <>
                      <span className="font-medium flex-1">{item.name}</span>
                      {item.badge && (
                        <Badge variant="secondary" className="h-5 px-2 text-xs">
                          {item.badge}
                        </Badge>
                      )}
                      <ChevronRight
                        className={`w-4 h-4 transition-transform ${itemActive === item.name ? 'text-primary-foreground' : 'text-muted-foreground group-hover:text-foreground'}`}
                      />
                    </>
                  )}
                  {/* Badge for collapsed state */}
                  {collapsed && item.badge && (
                    <div className="absolute -top-1 -right-1">
                      <Badge
                        variant="secondary"
                        className="h-4 w-4 p-0 text-xs flex items-center justify-center"
                      >
                        {item.badge}
                      </Badge>
                    </div>
                  )}
                </Button>
              ))}
            </nav>
          </div>

          {!collapsed && <Separator className="my-4" />}

          {/* Account Section */}
          <div className="mb-6">
            {!collapsed && (
              <h3 className="px-2 mb-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
                Account
              </h3>
            )}
            <nav className="space-y-1">
              {accountItems.map(function (item) {
                const googleItem = item.name.startsWith('Integrate');
                const integratedWithGoogle =
                  googleItem && currentUser?.integratedWithGoogle;
                if (integratedWithGoogle) {
                  item.name = 'Already Integrated';
                  item.click = () => {};
                }
                // item.name.startsWith("Integrate") ?
                // (
                //   {currentUser.inte}
                // )
                // :
                return (
                  <Button
                    key={item.name}
                    onClick={() => handleItemClick(item)}
                    title={collapsed ? item.name : undefined}
                    variant="ghost"
                    className={`
                 w-full flex items-center rounded-xl text-left transition-all duration-200 relative
                    ${collapsed ? 'justify-center p-2' : 'gap-3 px-3 py-2.5'}
                    ${
                      googleItem
                        ? integratedWithGoogle
                          ? 'bg-green-100 text-foreground  hover:bg-green-200 hover:text-foreground '
                          : 'bg-destructive/50 text-background hover:bg-destructive/90'
                        : itemActive === item.name
                          ? 'bg-primary text-primary-foreground shadow-md'
                          : 'hover:bg-secondary  text-muted-foreground hover:text-foreground'
                    }
                  `}
                  >
                    <span
                      className={`
                          text-base flex items-center
                          ${
                            googleItem
                              ? integratedWithGoogle
                                ? 'text-foreground   '
                                : 'text-background'
                              : itemActive === item.name
                                ? 'text-primary-foreground'
                                : 'text-muted-foreground group-hover:text-foreground'
                          }
                        `}
                    >
                      {item.icon}
                    </span>
                    {!collapsed && (
                      <>
                        <span className="font-medium flex-1">{item.name}</span>
                        {item.badge && (
                          <Badge
                            variant="destructive"
                            className="h-5 px-2 text-xs"
                          >
                            {item.badge}
                          </Badge>
                        )}
                        <ChevronRight className="w-4 h-4 text-muted-foreground group-hover:text-foreground" />
                      </>
                    )}
                    {/* Badge for collapsed state */}
                    {collapsed && item.badge && (
                      <div className="absolute -top-1 -right-1">
                        <Badge
                          variant="destructive"
                          className="h-4 w-4 p-0 text-xs flex items-center justify-center"
                        >
                          {item.badge}
                        </Badge>
                      </div>
                    )}
                  </Button>
                );
              })}
            </nav>
          </div>

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
