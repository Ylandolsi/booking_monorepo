import { AuthGuard } from '@/components/guards';
import Sidebar from '../navigation/side-bar/sidebar';
import { PageLoading } from '@/components/ui/index';
import { BottomNav } from '../navigation/bottom-nav';
import { useAuth } from '@/api/auth';
import { MainErrorFallback } from '@/components/errors';
import { useSideBar } from '@/stores';
import { Header } from '@/components/headers';

export function ContentLayout({ children }: { children: React.ReactNode }) {
  const { sidebarOpen, setSidebarOpen, collapsed, setCollapsed } = useSideBar();
  const { isLoading, error } = useAuth();

  if (error) return <MainErrorFallback />;
  if (isLoading) return <PageLoading />;

  return (
    <AuthGuard requireAuth={true} redirectTo="/auth/login">
      <div className="from-background/10 to-muted/10 flex h-screen bg-gradient-to-br">
        {/* Sidebar - Now at the layout level */}
        <Sidebar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} collapsed={collapsed} setCollapsed={setCollapsed} />

        {/* Main Content Area */}
        <div className="flex flex-1 flex-col overflow-hidden pb-10 lg:pb-0">
          <Header />
          {/* Page Content */}
          <main className="flex-1 overflow-auto px-4 py-6 pb-20 sm:px-6 sm:py-8 lg:px-8">{children}</main>

          {/* Bottom Navigation - Mobile Only */}
          <BottomNav />
        </div>
      </div>
    </AuthGuard>
  );
}
