import type { Item } from '@/components/navigation/side-bar';
import { Badge, Button } from '@/components/ui';
import { useAppNavigation } from '@/hooks';
import { ChevronRight, Unplug } from 'lucide-react';
import { MdPayment } from 'react-icons/md';

export function AccountSection(props: {
  handleItemClick: (item: Item) => void;
  itemActive: string;
  collapsed: boolean;
}) {
  const nav = useAppNavigation();
  const accountItems: Item[] = [
    {
      icon: <Unplug size={20} />,
      click: () => nav.goToIntegrations(),
      name: 'Integrations',
    },
    {
      icon: <MdPayment size={20} />,
      click: () => {
        nav.goTo('');
      },
      name: 'Payout',
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
  return (
    <div className="mb-6">
      {!props.collapsed && (
        <h3 className="px-2 mb-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
          Account
        </h3>
      )}
      <nav className="space-y-1">
        {accountItems.map(function (item) {
          return (
            <Button
              key={item.name}
              onClick={() => props.handleItemClick(item)}
              title={props.collapsed ? item.name : undefined}
              variant="ghost"
              className={`
                 w-full flex items-center rounded-xl text-left transition-all duration-200 relative ${props.collapsed ? 'justify-center p-2' : 'gap-3 px-3 py-2.5'}
                    ${props.itemActive === item.name ? 'bg-primary text-primary-foreground shadow-md' : 'hover:bg-accent  text-muted-foreground hover:text-foreground'}
                    }
                  `}
            >
              <span
                className={`
                          text-base flex items-center
                          ${props.itemActive === item.name ? 'text-primary-foreground' : 'text-muted-foreground group-hover:text-foreground'}
                        `}
              >
                {item.icon}
              </span>
              {!props.collapsed && (
                <>
                  <span className="font-medium flex-1">{item.name}</span>
                  {item.badge && (
                    <Badge variant="destructive" className="h-5 px-2 text-xs">
                      {item.badge}
                    </Badge>
                  )}
                  <ChevronRight className="w-4 h-4 text-muted-foreground group-hover:text-foreground" />
                </>
              )}
              {/* Badge for collapsed state */}
              {props.collapsed && item.badge && (
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
  );
}
