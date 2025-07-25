import { ContentLayout } from '@/components/layouts';
import { Header } from '@/features/profile/components/headers/header';
import { Sections } from '@/features/profile/components/sections/sections';

export function MyPorfile() {
  return (
    <ContentLayout>
      <div className="min-h-screen bg-gradient-to-br from-background to-muted">
        <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-6 sm:py-8">
          <div className="space-y-6">
            <Header />
            <div className="bg-card rounded-2xl shadow-lg border border-border/50 overflow-hidden">
              <Sections />
            </div>
          </div>
        </div>
      </div>
    </ContentLayout>
  );
  
}
