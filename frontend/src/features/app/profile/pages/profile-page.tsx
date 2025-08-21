import {
  GlobalProfileEditDialog,
  Header,
  Sections,
} from '@/features/app/profile';

export function Profile() {
  return (
    <>
      <div className="max-w-6xl mx-auto">
        <div className="space-y-6">
          <Header />
          <div className="bg-card rounded-2xl shadow-lg border border-border/50 overflow-hidden">
            <Sections />
          </div>
        </div>
      </div>
      <GlobalProfileEditDialog />
    </>
  );
}
