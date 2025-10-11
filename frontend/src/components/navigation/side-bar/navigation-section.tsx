import type { Item } from '@/components/navigation/side-bar';
import type { User } from '@/api/auth';
import { Button } from '@/components/ui';
import { useAppNavigation } from '@/hooks';
import { Calendar, ChevronRight, Home, Store } from 'lucide-react';
import { routes } from '@/config';

export function NavigationSection(props: { handleItemClick: (item: Item) => void; itemActive: string; collapsed: boolean; currentUser: User }) {
  const nav = useAppNavigation();
  const navigationItems: Item[] = [
    {
      name: 'Edit Store',
      icon: <Store size={20} />,
      click: () => nav.goTo({ to: routes.to.store.index() + '/' }),
    },
    {
      name: 'Meetings',
      icon: <Calendar size={20} />,
      click: () => {
        nav.goToMeets();
      },
    },
    // {
    //   name: 'Search',
    //   icon: <Search size={20} />,
    //   click: () => nav.goToApp(),
    // },
  ];
  return (
    <div className="mb-6">
      {!props.collapsed && <h3 className="text-accent-foreground mb-3 px-2 text-xs font-semibold tracking-wider uppercase">Navigation</h3>}
      <nav className="space-y-1">
        {navigationItems.map((item) => (
          <Button
            variant={'ghost'}
            key={item.name}
            onClick={() => props.handleItemClick(item)}
            className={`group relative flex w-full items-center rounded-lg text-left transition-all duration-200 ${props.collapsed ? 'justify-center p-2' : 'gap-3 px-3 py-2.5'} ${props.itemActive == item.name ? 'bg-primary text-primary-foreground shadow-md' : 'hover:bg-accent text-accent-foreground hover:text-foreground'} `}
            title={props.collapsed ? item.name : undefined}
          >
            <span>{item.icon}</span>
            {!props.collapsed && (
              <>
                <span className="flex-1 font-medium">{item.name}</span>
                <ChevronRight
                  className={`h-4 w-4 transition-transform ${props.itemActive === item.name ? 'text-primary-foreground' : 'text-accent-foreground group-hover:text-foreground'}`}
                />
              </>
            )}
          </Button>
        ))}
      </nav>
    </div>
  );
}
