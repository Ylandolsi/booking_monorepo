import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { registerInputSchema, AUTH_PLACEHOLDERS, type RegisterInput } from '@/pages/auth';

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

import { ROUTE_PATHS } from '@/config';
import { googleOIDC, useRegister } from '@/api/auth';

type RegisterFormProps = {
  OnRegiterSuccess: () => void;
};

export const RegisterForm = ({ OnRegiterSuccess }: RegisterFormProps) => {
  const register = useRegister({ onSuccess: OnRegiterSuccess });

  const form = useForm<RegisterInput>({
    resolver: zodResolver(registerInputSchema),
    defaultValues: {
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: '',
    },
  });

  function onSubmit(values: RegisterInput) {
    register.mutate(values);
  }

  return (
    <>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 lg:space-y-6">
            <FormField
              control={form.control}
              name="firstName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>First Name</FormLabel>
                  <FormControl>
                    <Input placeholder={AUTH_PLACEHOLDERS.firstName} {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="lastName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Last Name</FormLabel>
                  <FormControl>
                    <Input placeholder={AUTH_PLACEHOLDERS.lastName} {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
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
            <FormField
              control={form.control}
              name="confirmPassword"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Confirm Password</FormLabel>
                  <FormControl>
                    <PasswordInput placeholder={AUTH_PLACEHOLDERS.confirmPassword} {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <Button type="submit" className="w-full" disabled={register.isPending}>
              {register.isPending ? 'Registering...' : 'Register'}
            </Button>
          </form>
        </Form>
      </CardContent>
      <CardFooter className="flex-col gap-2">
        <Button variant="outline" className="w-full" onClick={() => googleOIDC()}>
          Register with Google
        </Button>
      </CardFooter>
      <div className="flex flex-col items-center">
        <div className="text-muted-foreground mt-4 text-center text-sm">
          Already have an account?{' '}
          <Link className="text-primary hover:underline" to={ROUTE_PATHS.AUTH.LOGIN}>
            Login
          </Link>
        </div>
      </div>
    </>
  );
};
