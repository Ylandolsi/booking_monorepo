import { Link } from '@/components/ui';
import { routes } from '@/config/routes';

export const HomePage = () => {
  return (
    <div className="grid min-h-screen grid-rows-[20px_1fr_20px] items-center justify-items-center gap-16 p-8 pb-20 sm:p-20">
      <main className="row-start-2 flex flex-col items-center gap-8 sm:items-start">
        <h1 className="text-4xl font-bold">Welcome to Booking App</h1>
        <div className="flex flex-col items-center gap-4 sm:flex-row">
          <Link
            to={routes.to.auth.login() as any}
            className="bg-foreground text-background flex h-10 items-center justify-center gap-2 rounded-full border border-solid border-transparent px-4 text-sm font-medium transition-colors hover:bg-[#383838] sm:h-12 sm:w-auto sm:px-5 sm:text-base"
          >
            Login
          </Link>
          <Link
            to={routes.to.auth.register() as any}
            className="flex h-10 w-full items-center justify-center rounded-full border border-solid border-black/[.08] px-4 text-sm font-medium transition-colors hover:border-transparent hover:bg-[#f2f2f2] sm:h-12 sm:w-auto sm:px-5 sm:text-base"
          >
            Register
          </Link>
        </div>
      </main>
    </div>
  );
};
