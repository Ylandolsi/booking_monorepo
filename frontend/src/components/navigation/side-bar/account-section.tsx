import type { Item } from '@/components/navigation/side-bar';
import { Badge, Button } from '@/components/ui';
import { useAppNavigation } from '@/hooks';
import { ChartBar, ChevronRight, DollarSign, Unplug } from 'lucide-react';
import { MdPayment } from 'react-icons/md';

export function AccountSection(props: { handleItemClick: (item: Item) => void; itemActive: string; collapsed: boolean }) {
  const nav = useAppNavigation();
  const accountItems: Item[] = [
    {
      icon: <Unplug size={20} />,
      click: () => nav.goToIntegrations(),
      name: 'Integrations',
    },
    {
      icon: <ChartBar size={20} />,
      click: () => nav.goTo({ to: '/app/statistics' }),
      name: 'Statistics',
    },
    {
      icon: <MdPayment size={20} />,
      click: () => {
        nav.goToPayout();
      },
      name: 'Payout',
    },
    {
      name: 'Orders',
      icon: <DollarSign size={20} />,
      click: () => {
        nav.goTo({ to: '/app/orders' });
      },
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
      {!props.collapsed && <h3 className="text-accent-foreground mb-3 px-2 text-xs font-semibold tracking-wider uppercase">Account</h3>}
      <nav className="space-y-1">
        {accountItems.map(function (item) {
          return (
            <Button
              key={item.name}
              onClick={() => props.handleItemClick(item)}
              title={props.collapsed ? item.name : undefined}
              variant="ghost"
              className={`relative flex w-full items-center rounded-xl text-left transition-all duration-200 ${props.collapsed ? 'justify-center p-2' : 'gap-3 px-3 py-2.5'} ${props.itemActive === item.name ? 'bg-primary text-primary-foreground shadow-md' : 'hover:bg-accent text-accent-foreground hover:text-foreground'} }`}
            >
              <span>{item.icon}</span>
              {!props.collapsed && (
                <>
                  <span className="flex-1 font-medium">{item.name}</span>
                  {item.badge && (
                    <Badge variant="destructive" className="h-5 px-2 text-xs">
                      {item.badge}
                    </Badge>
                  )}
                  <ChevronRight className="text-accent-foreground group-hover:text-foreground h-4 w-4" />
                </>
              )}
              {/* Badge for collapsed state */}
              {props.collapsed && item.badge && (
                <div className="absolute -top-1 -right-1">
                  <Badge variant="destructive" className="flex h-4 w-4 items-center justify-center p-0 text-xs">
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
