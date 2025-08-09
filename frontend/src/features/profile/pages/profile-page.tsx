import { ContentLayout } from '@/components/layouts';
import { GlobalProfileEditDialog, Header, Sections } from '@/features/profile';

export function Profile() {
  return (
    <ContentLayout>
      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-6 sm:py-8">
        <div className="space-y-6">
          <Header />
          <div className="bg-card rounded-2xl shadow-lg border border-border/50 overflow-hidden">
            <Sections />
          </div>
        </div>
      </div>
      <GlobalProfileEditDialog />
    </ContentLayout>
  );
}
