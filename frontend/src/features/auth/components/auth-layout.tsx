import * as React from 'react';
import { useSearch } from '@tanstack/react-router';
import { Logo } from '../../../components/logo';
import { Card, CardHeader, Link } from '@/components/ui/index';
import { AuthGuard } from '@/components/guards';

type LayoutProps = {
  children: React.ReactNode;
};

export const AuthLayout = ({ children }: LayoutProps) => {
  const search = useSearch({ strict: false });
  const redirectTo = (search.redirectTo as string) || './app';

  return (
    <AuthGuard authPage={true} redirectTo={redirectTo}>
      <div className="bg-white-50 flex min-h-screen flex-col justify-center gap-8 px-2 py-12 sm:px-6 lg:px-8">
        <Card className="mx-auto w-full max-w-md border-none shadow-lg">
          <CardHeader>
            <div className="flex justify-center">
              <Link className="flex items-center text-white" to="/">
                <Logo />
              </Link>
            </div>
          </CardHeader>
          {children}
        </Card>
      </div>
    </AuthGuard>
  );
};
