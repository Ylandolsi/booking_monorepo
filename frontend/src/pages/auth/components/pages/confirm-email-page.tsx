import {
  CardContent,
  CardDescription,
  CardFooter,
  Link,
} from '@/components/ui';

type ConfirmEmailPageProps = {
  onSuccess: () => void;
};

export const ConfirmEmailPage = ({ onSuccess }: ConfirmEmailPageProps) => {
  return (
    <>
      <CardDescription className="text-center sm:mx-auto sm:w-full sm:max-w-md">
        <h2 className="text-2xl font-semibold tracking-tight">
          Check Your Email
        </h2>
        <p className="text-muted-foreground mx-2 mt-2">
          We have sent a verification email to your account. Please check your
          email and click the verification link.
        </p>
      </CardDescription>
      <CardContent></CardContent>
      <CardFooter className="flex-col gap-2"></CardFooter>
      <Link
        className="text-primary flex justify-center hover:underline"
        href="/auth/login"
      >
        Back to login
      </Link>
    </>
  );
};
