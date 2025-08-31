import type { Item } from '@/components/navigation/side-bar/sidebar';
import { Button } from '@/components/ui';
import { useAppNavigation } from '@/hooks';
import { FaMoneyCheck } from 'react-icons/fa';

export function AdminSection(props: {
  handleItemClick: (item: Item) => void;
  itemActive: string;
  collapsed: boolean;
}) {
  const nav = useAppNavigation();
  const adminItems: Item[] = [
    {
      name: 'Payouts Requests',
      icon: <FaMoneyCheck size={20} />,
      click: () => nav.goToApp(),
    },
  ];

  return (
    <div className="mb-6">
      {!props.collapsed && (
        <h3 className="px-2 mb-3 text-xs font-semibold text-muted-foreground uppercase tracking-wider">
          Admin
        </h3>
      )}
      <nav className="space-y-1">
        {adminItems.map(function (item) {
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
              <span>{item.icon}</span>
              {!props.collapsed && (
                <>
                  <span className="font-medium flex-1">{item.name}</span>
                </>
              )}
            </Button>
          );
        })}
      </nav>
    </div>
  );
}
