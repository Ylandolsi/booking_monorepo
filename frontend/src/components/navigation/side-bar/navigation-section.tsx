import type { Item } from '@/components/navigation/side-bar';
import type { User } from '@/types/api';
import { Button } from '@/components/ui';
import { useAppNavigation } from '@/hooks';
import {
  Badge,
  Calendar,
  ChevronRight,
  Home,
  Search,
  Timer,
  UserIcon,
} from 'lucide-react';
import { GiTeacher } from 'react-icons/gi';

export function NavigationSection(props: {
  handleItemClick: (item: Item) => void;
  itemActive: string;
  collapsed: boolean;
  currentUser: User;
}) {
  const nav = useAppNavigation();
  const navigationItems: Item[] = [
    {
      name: 'Home',
      icon: <Home size={20} />,
      click: () => nav.goToApp(),
    },
    {
      name: 'Profile',
      icon: <UserIcon size={20} />,
      click: () => nav.goToProfile(props.currentUser?.slug || ''),
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
    // {
    //   name: 'Search',
    //   icon: <Search size={20} />,
    //   click: () => nav.goToApp(),
    // },
  ];
  return (
    <div className="mb-6">
      {!props.collapsed && (
        <h3 className="px-2 mb-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
          Navigation
        </h3>
      )}
      <nav className="space-y-1">
        {navigationItems.map((item) => (
          <Button
            variant={'ghost'}
            key={item.name}
            onClick={() => props.handleItemClick(item)}
            className={`
                    w-full flex items-center rounded-lg text-left transition-all duration-200 group relative
                    ${props.collapsed ? 'justify-center p-2' : 'gap-3 px-3 py-2.5'}
                    ${props.itemActive == item.name ? 'bg-primary text-primary-foreground shadow-md' : 'hover:bg-accent text-muted-foreground hover:text-foreground'}
                  `}
            title={props.collapsed ? item.name : undefined}
          >
            <span>{item.icon}</span>
            {!props.collapsed && (
              <>
                <span className="font-medium flex-1">{item.name}</span>
                {item.badge && (
                  <Badge className="h-5 px-2 text-xs">{item.badge}</Badge>
                )}
                <ChevronRight
                  className={`w-4 h-4 transition-transform ${props.itemActive === item.name ? 'text-primary-foreground' : 'text-muted-foreground group-hover:text-foreground'}`}
                />
              </>
            )}
            {/* Badge for collapsed state */}
            {props.collapsed && item.badge && (
              <div className="absolute -top-1 -right-1">
                <Badge className="h-4 w-4 p-0 text-xs flex items-center justify-center">
                  {item.badge}
                </Badge>
              </div>
            )}
          </Button>
        ))}
      </nav>
    </div>
  );
}
