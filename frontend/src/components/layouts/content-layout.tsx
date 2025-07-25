import { AuthGuard } from '@/features/auth/components/auth-guard';
import { Logo } from '@/components/logo';
import Sidebar from './sidebar';
import { Button, Badge, Spinner } from '@/components/ui/index';
import { LazyImage } from '@/utils/lazy-image';
import { Menu, Bell } from 'lucide-react';
import { useState } from 'react';
import { useNavigate } from '@tanstack/react-router';
import { BottomNav } from '../navigation/bottom-nav';
import { useAuth } from '@/features/auth/hooks';
import { MainErrorFallback } from '../errors/main';

export function ContentLayout({ children }: { children: React.ReactNode }) {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const { currentUser, isLoading, error } = useAuth();
  const navigate = useNavigate();
  if (error) return <MainErrorFallback />;
  if (isLoading) return <Spinner />;

  return (
    <AuthGuard requireAuth={true} redirectTo="/auth/login">
      <>
        {/* Header */}
        <header className="sticky top-0 left-0 right-0 z-30 bg-card border-b border-border shadow-sm">
          <div className="flex items-center justify-between px-4 py-3">
            {/* Left side - Logo and Menu */}
            <div className="flex items-center gap-3">
              <Button
                variant="ghost"
                size="sm"
                onClick={() => setSidebarOpen(!sidebarOpen)}
                className="h-10 w-10 p-0 hover:bg-muted "
              >
                <Menu className="w-5 h-5" />
              </Button>
              <Logo className="h-8" />
            </div>

            {/* Right side - Profile and Notifications */}
            <div className="flex items-center gap-2">
              <Button
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
              </Button>

              <Button
                variant="ghost"
                size="sm"
                onClick={() => navigate({ to: `/members/${currentUser?.id}` })}
                className="h-10 px-2 hover:bg-muted"
              >
                <LazyImage
                  className="w-7 h-7 rounded-full ring-2 ring-primary/20 object-cover"
                  src={
                    currentUser?.profilePictureUrl ||
                    'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
                  }
                  alt="profile-pic"
                  placeholder={
                    currentUser?.profilePictureUrl ||
                    'https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250'
                  }
                />
              </Button>
            </div>
          </div>
        </header>

        {/* Main Content */}
        <main className="min-h-[calc(100vh-8rem)] pb-20 lg:pb-4">
          {children}
        </main>

        {/* Bottom Navigation - Mobile Only */}
        <BottomNav />

        {/* Sidebar */}
        <Sidebar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
      </>
    </AuthGuard>
  );
}
