import { routes } from './routes';

export const paths = {
  home: {
    getHref: () => routes.to.home(),
  },
  auth: {
    register: {
      getHref: (redirectTo?: string | null | undefined) =>
        routes.to.auth.register(redirectTo ? { redirectTo } : undefined),
    },
    login: {
      getHref: (redirectTo?: string | null | undefined) =>
        routes.to.auth.login(redirectTo ? { redirectTo } : undefined),
    },
    verificationEmail: {
      getHref: (redirectTo?: string | null | undefined) =>
        routes.to.auth.emailVerification(
          redirectTo ? { redirectTo } : undefined,
        ),
    },
    emailVerified: {
      getHref: (redirectTo?: string | null | undefined) =>
        routes.to.auth.emailVerificationVerified(
          redirectTo ? { redirectTo } : undefined,
        ),
    },
    forgotPassword: {
      getHref: (redirectTo?: string | null | undefined) =>
        routes.to.auth.forgotPassword(redirectTo ? { redirectTo } : undefined),
    },
    resetPassword: {
      getHref: (params?: {
        redirectTo?: string;
        email?: string;
        token?: string;
      }) => routes.to.auth.resetPassword(params),
    },
  },
  app: {
    root: {
      getHref: () => routes.to.app.root(),
    },
    discussions: {
      getHref: () => '/app/discussions',
    },
    discussion: {
      getHref: (id: string) => `/app/discussions/${id}`,
    },
    users: {
      getHref: () => '/app/users',
    },
    profile: {
      getHref: () => '/app/profile',
    },
  },
  booking: {
    session: {
      getHref: (mentorSlug: string) => routes.to.booking.session(mentorSlug),
    },
    demo: {
      getHref: (mentorSlug: string) => routes.to.booking.demo(mentorSlug),
    },
    enhanced: {
      getHref: (mentorSlug: string) => routes.to.booking.enhanced(mentorSlug),
    },
    test: {
      getHref: (mentorSlug: string) => routes.to.booking.test(mentorSlug),
    },
  },
  mentor: {
    become: {
      getHref: () => routes.to.mentor.become(),
    },
    setSchedule: {
      getHref: () => routes.to.mentor.setSchedule(),
    },
  },
  profile: {
    user: {
      getHref: (userSlug: string) => routes.to.profile.user(userSlug),
    },
  },
  test: {
    mentorRequired: {
      getHref: () => routes.to.test.mentorRequired(),
    },
    img: {
      getHref: () => routes.to.test.img(),
    },
    dashboard: {
      getHref: () => routes.to.test.dashboard(),
    },
    bookingDemo: {
      getHref: () => routes.to.test.bookingDemo(),
    },
    already: {
      getHref: () => routes.to.test.already(),
    },
  },
  errorExp: {
    simpleLoading: {
      getHref: () => routes.to.errorExp.simpleLoading(),
    },
    advancedLoading: {
      getHref: () => routes.to.errorExp.advancedLoading(),
    },
  },
  public: {
    discussion: {
      getHref: (id: string) => `/public/discussions/${id}`,
    },
  },
  unauthorized: {
    getHref: () => routes.to.unauthorized(),
  },
} as const;
