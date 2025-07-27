import { useEffect, useState } from 'react';
import {
  CardContent,
  CardDescription,
  CardFooter,
  Link,
  Spinner,
} from '@/components/ui';
import { verifyEmail } from '../../api/verify-email';

type EmailVerificationPageProps = {
  onSuccess: () => void;
  token: string | undefined;
  email: string | undefined;
};

export const EmailVerificationPage = ({
  onSuccess,
  token,
  email,
}: EmailVerificationPageProps) => {
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!token || !email) return;

    let isCancelled = false;

    const verify = async () => {
      setLoading(true);
      setError(null);
      try {
        await verifyEmail({ token, email });
        if (!isCancelled) {
          onSuccess();
        }
      } catch (_) {
        if (!isCancelled) {
          setError('Verification failed. Please try again.');
        }
      } finally {
        if (!isCancelled) {
          setLoading(false);
        }
      }
    };

    if (!isCancelled) verify();

    return () => {
      isCancelled = true;
    };
  }, [token, email, onSuccess]);

  return (
    <>
      <CardDescription className="text-center sm:mx-auto sm:w-full sm:max-w-md">
        <h2 className="text-2xl font-semibold tracking-tight">
          Verify Your Email
        </h2>
        <p className="text-muted-foreground mx-2 mt-2">
          We sent a verification email to your account. Please check your inbox
          and click the link to confirm.
        </p>
        {loading && <Spinner />}
        {error && <p className="mt-4 text-red-500">{error}</p>}
      </CardDescription>

      <CardContent></CardContent>
      <CardFooter className="flex-col gap-2">
        {error && (
          <button
            className="text-sm text-blue-600 hover:underline"
            onClick={() => {
              if (token && email) {
                setError(null);
                setLoading(true);
                verifyEmail({ token, email })
                  .then(() => {
                    setLoading(false);
                    onSuccess();
                  })
                  .catch(() => {
                    setLoading(false);
                    setError('Verification failed. Please try again.');
                  });
              }
            }}
          >
            Retry Verification
          </button>
        )}
      </CardFooter>

      <Link
        className="text-primary mt-2 flex justify-center hover:underline"
        href="/auth/login"
      >
        Back to login
      </Link>
    </>
  );
};
