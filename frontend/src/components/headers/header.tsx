import { Logo } from '@/components/logo';
import { Badge, Button } from '@/components/ui';
import { useAuth } from '@/features/auth';
import { useSideBar } from '@/stores';
import { LazyImage } from '@/utils';
import { Bell, Menu } from 'lucide-react';
import { useAppNavigation } from '@/hooks';

export function Header() {
  const { setSidebarOpen } = useSideBar();
  const { currentUser } = useAuth();
  const navigate = useAppNavigation();

  if (!currentUser || currentUser == undefined) return null;

  return (
    <header className="sticky top-0 z-30 bg-card border-b border-border shadow-sm">
      <div className="flex items-center justify-between px-4 py-3">
        {/* Left side - Logo and Menu (Mobile only) */}
        <div className="flex items-center gap-3">
          <Button
            variant="ghost"
            size="sm"
            onClick={() => setSidebarOpen(true)}
            className="h-10 w-10 p-0 hover:bg-muted lg:hidden"
          >
            <Menu className="w-5 h-5" />
          </Button>
          <Logo className="h-8" />
        </div>

        {/* Right side - Profile and Notifications */}
        {
          <div className="flex items-center gap-2 lg:hidden">
            {/* <Button
              variant="ghost"
              size="sm"
              className="h-10 w-10 p-0 hover:bg-muted relative"
            >
              <Bell className="w-5 h-5" />
              <Badge
                variant="destructive"
                className="absolute -top-1 -right-1 h-5 w-5 p-0 text-xs flex items-center justify-center"
              >
                3
              </Badge>
            </Button> */}

            <Button
              variant="ghost"
              size="sm"
              onClick={() => navigate.goToProfile(currentUser.slug)}
              className="h-10 px-2 hover:bg-muted"
            >
              <LazyImage
                className="w-7 h-7 rounded-full ring-2 ring-primary/20 object-cover"
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
            </Button>
          </div>
        }
      </div>
    </header>
  );
}
