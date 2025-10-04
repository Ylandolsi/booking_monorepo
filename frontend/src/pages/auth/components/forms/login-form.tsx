import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { loginInputSchema, type LoginInput, AUTH_PLACEHOLDERS } from '@/pages/auth';
import { routes } from '@/config/routes';

import {
  Input,
  Button,
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  CardContent,
  CardFooter,
  Link,
  PasswordInput,
} from '@/components/ui';
import { googleOIDC, useLogin } from '@/api/auth';

type LoginFormProps = {
  onSuccess: () => void;
};

export const LoginForm = ({ onSuccess }: LoginFormProps) => {
  const login = useLogin({ onSuccess });

  const form = useForm<LoginInput>({
    resolver: zodResolver(loginInputSchema),
    defaultValues: {
      email: '',
      password: '',
    },
  });

  function onSubmit(values: LoginInput) {
    login.mutate(values);
  }

  return (
    <>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 lg:space-y-6">
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Email</FormLabel>
                  <FormControl>
                    <Input type="email" placeholder={AUTH_PLACEHOLDERS.email} {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Password</FormLabel>
                  <FormControl>
                    <PasswordInput placeholder={AUTH_PLACEHOLDERS.password} {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <Button type="submit" className="w-full" disabled={login.isPending}>
              {login.isPending ? 'Logging in...' : 'Login'}
            </Button>
          </form>
        </Form>
      </CardContent>
      <CardFooter className="flex-col gap-2">
        <Button variant="outline" className="w-full" onClick={() => googleOIDC()}>
          Login with Google
        </Button>
      </CardFooter>
      <div className="flex flex-col items-center">
        <div className="text-foreground mt-4 ml-2 text-center text-sm font-bold">
          <Link className="text-primary/70 hover:text-primary" href={routes.to.auth.forgotPassword()}>
            Forget your password?
          </Link>
        </div>
        <div className="text-muted-foreground mt-4 text-center text-sm">
          Don't have an account?{' '}
          <Link className="text-primary hover:underline" href={routes.to.auth.register()}>
            Register
          </Link>
        </div>
      </div>
    </>
  );
};
