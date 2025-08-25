import {
  Calendar,
  House,
  MessageCircle,
  Search,
  UserSearch,
} from 'lucide-react';
import { Badge } from '@/components/ui';
import { useAppNavigation } from '@/hooks/use-navigation';

export function BottomNav() {
  const nav = useAppNavigation();

  const bottomNavItems = [
    {
      icon: <House size={20} />,
      label: 'Home',
      onClick: () => nav.goToApp(),
      active: true,
    },
    {
      icon: <Calendar size={20} />,
      label: 'Bookings',
      onClick: () => {},
      badge: '3',
      active: false,
    },
    {
      icon: <Search size={20} />,
      label: 'Search',
      onClick: () => {},
      active: false,
    },
    // {
    //   icon: <MessageCircle size={20} />,
    //   label: 'Messages',
    //   onClick: () => {},
    //   badge: '2',
    //   active: false,
    // },
    // {
    //   icon: <UserSearch size={20} />,
    //   label: 'Discover',
    //   onClick: () => {},
    //   active: false,
    // },
  ];
  return (
    <nav className="fixed bottom-0 left-0 right-0 bg-card border-t border-border shadow-lg lg:hidden z-30">
      <div className="flex items-center justify-around px-2 py-2">
        {bottomNavItems.map((item, index) => (
          <button
            key={index}
            onClick={item.onClick}
            className={`flex flex-col cursor-pointer items-center gap-1 px-3 py-2 rounded-lg transition-all duration-200 relative ${
              item.active
                ? 'text-primary bg-primary/10'
                : 'text-muted-foreground hover:text-foreground hover:bg-muted'
            }`}
          >
            <div className="relative">
              {item.icon}
              {item.badge && (
                <Badge
                  variant="destructive"
                  className="absolute -top-2 -right-2 h-4 w-4 p-0 text-xs flex items-center justify-center"
                >
                  {item.badge}
                </Badge>
              )}
            </div>
            <span className="text-xs font-medium">{item.label}</span>
          </button>
        ))}
      </div>
    </nav>
  );
}
