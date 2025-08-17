import { CardDescription, CardFooter, Link } from '@/components/ui';

export function VerificationEmailDonePage() {
  return (
    <>
      <CardDescription className="text-center sm:mx-auto sm:w-full sm:max-w-md">
        <h2 className="text-2xl font-semibold tracking-tight">
          Your email is verified
        </h2>
        <Link
          className="text-primary mt-2 flex justify-center font-bold hover:underline"
          to="/auth/login"
        >
          Login to your account
        </Link>
      </CardDescription>
      <CardFooter className="flex-col gap-2"></CardFooter>
    </>
  );
}
