import { Link } from '@/components/ui';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { routes } from '@/config/routes';
import { Calendar, Users, Clock, Star, ArrowRight, BookOpen, Zap, Shield } from 'lucide-react';

export const LandingPage = () => {
  return (
    <div className="min-h-screen bg-gradient-to-br from-background to-muted">
      {/* Hero Section */}
      <section className="relative px-4 py-20 sm:px-6 lg:px-8">
        <div className="mx-auto max-w-7xl">
          <div className="text-center">
            <h1 className="text-4xl font-bold tracking-tight text-foreground sm:text-6xl lg:text-7xl">
              Connect with
              <span className="bg-gradient-to-r from-primary to-chart-2 bg-clip-text text-transparent">
                {' '}Expert Mentors
              </span>
            </h1>
            <p className="mx-auto mt-6 max-w-2xl text-lg leading-8 text-muted-foreground sm:text-xl">
              Book personalized mentoring sessions with industry experts. Accelerate your learning and achieve your goals with one-on-one guidance.
            </p>
            <div className="mt-10 flex items-center justify-center gap-x-6">
              <Button size="lg" asChild className="rounded-full px-8">
                <Link to={routes.paths.AUTH.REGISTER as any}>
                  Get Started
                  <ArrowRight className="ml-2 h-4 w-4" />
                </Link>
              </Button>
              <Button variant="outline" size="lg" asChild className="rounded-full px-8">
                <Link to={routes.paths.AUTH.LOGIN as any}>
                  Sign In
                </Link>
              </Button>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="px-4 py-20 sm:px-6 lg:px-8">
        <div className="mx-auto max-w-7xl">
          <div className="text-center mb-16">
            <h2 className="text-3xl font-bold tracking-tight text-foreground sm:text-4xl">
              Why Choose Our Platform?
            </h2>
            <p className="mt-4 text-lg text-muted-foreground">
              Everything you need for successful mentoring sessions
            </p>
          </div>
          
          <div className="grid gap-8 sm:grid-cols-2 lg:grid-cols-3">
            <Card className="relative overflow-hidden border-0 bg-card/50 backdrop-blur-sm">
              <CardHeader>
                <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-primary/10">
                  <Calendar className="h-6 w-6 text-primary" />
                </div>
                <CardTitle className="text-xl">Easy Scheduling</CardTitle>
              </CardHeader>
              <CardContent>
                <p className="text-muted-foreground">
                  Book sessions that fit your schedule. Our intuitive calendar system makes it simple to find available slots.
                </p>
              </CardContent>
            </Card>

            <Card className="relative overflow-hidden border-0 bg-card/50 backdrop-blur-sm">
              <CardHeader>
                <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-chart-2/10">
                  <Users className="h-6 w-6 text-chart-2" />
                </div>
                <CardTitle className="text-xl">Expert Mentors</CardTitle>
              </CardHeader>
              <CardContent>
                <p className="text-muted-foreground">
                  Connect with verified industry professionals who are passionate about sharing their knowledge and experience.
                </p>
              </CardContent>
            </Card>

            <Card className="relative overflow-hidden border-0 bg-card/50 backdrop-blur-sm">
              <CardHeader>
                <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-chart-3/10">
                  <Clock className="h-6 w-6 text-chart-3" />
                </div>
                <CardTitle className="text-xl">Flexible Sessions</CardTitle>
              </CardHeader>
              <CardContent>
                <p className="text-muted-foreground">
                  Choose session duration that works for you. From quick 30-minute consultations to deep-dive 2-hour sessions.
                </p>
              </CardContent>
            </Card>

            <Card className="relative overflow-hidden border-0 bg-card/50 backdrop-blur-sm">
              <CardHeader>
                <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-chart-4/10">
                  <BookOpen className="h-6 w-6 text-chart-4" />
                </div>
                <CardTitle className="text-xl">Personalized Learning</CardTitle>
              </CardHeader>
              <CardContent>
                <p className="text-muted-foreground">
                  Get tailored advice and guidance based on your specific goals, experience level, and learning style.
                </p>
              </CardContent>
            </Card>

            <Card className="relative overflow-hidden border-0 bg-card/50 backdrop-blur-sm">
              <CardHeader>
                <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-chart-5/10">
                  <Zap className="h-6 w-6 text-chart-5" />
                </div>
                <CardTitle className="text-xl">Instant Access</CardTitle>
              </CardHeader>
              <CardContent>
                <p className="text-muted-foreground">
                  Join sessions instantly through our integrated platform. No additional software installation required.
                </p>
              </CardContent>
            </Card>

            <Card className="relative overflow-hidden border-0 bg-card/50 backdrop-blur-sm">
              <CardHeader>
                <div className="flex h-12 w-12 items-center justify-center rounded-lg bg-primary/10">
                  <Shield className="h-6 w-6 text-primary" />
                </div>
                <CardTitle className="text-xl">Secure & Reliable</CardTitle>
              </CardHeader>
              <CardContent>
                <p className="text-muted-foreground">
                  Your sessions and data are protected with enterprise-grade security. Focus on learning, we handle the rest.
                </p>
              </CardContent>
            </Card>
          </div>
        </div>
      </section>

      {/* Stats Section */}
      <section className="px-4 py-20 sm:px-6 lg:px-8">
        <div className="mx-auto max-w-7xl">
          <Card className="border-0 bg-gradient-to-r from-primary/5 to-chart-2/5 backdrop-blur-sm">
            <CardContent className="p-12">
              <div className="grid gap-8 sm:grid-cols-2 lg:grid-cols-4 text-center">
                <div>
                  <div className="text-3xl font-bold text-foreground sm:text-4xl">500+</div>
                  <div className="mt-2 text-muted-foreground">Expert Mentors</div>
                </div>
                <div>
                  <div className="text-3xl font-bold text-foreground sm:text-4xl">10k+</div>
                  <div className="mt-2 text-muted-foreground">Sessions Completed</div>
                </div>
                <div>
                  <div className="text-3xl font-bold text-foreground sm:text-4xl">95%</div>
                  <div className="mt-2 text-muted-foreground">Satisfaction Rate</div>
                </div>
                <div className="flex items-center justify-center gap-1">
                  <div className="text-3xl font-bold text-foreground sm:text-4xl">4.9</div>
                  <Star className="h-8 w-8 fill-yellow-400 text-yellow-400" />
                  <div className="mt-2 text-muted-foreground self-end">Average Rating</div>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>
      </section>

      {/* CTA Section */}
      <section className="px-4 py-20 sm:px-6 lg:px-8">
        <div className="mx-auto max-w-4xl text-center">
          <h2 className="text-3xl font-bold tracking-tight text-foreground sm:text-4xl">
            Ready to accelerate your growth?
          </h2>
          <p className="mt-4 text-lg text-muted-foreground">
            Join thousands of learners who are already achieving their goals with expert guidance.
          </p>
          <div className="mt-10 flex items-center justify-center gap-x-6">
            <Button size="lg" asChild className="rounded-full px-12">
              <Link to={routes.paths.AUTH.REGISTER as any}>
                Start Learning Today
                <ArrowRight className="ml-2 h-4 w-4" />
              </Link>
            </Button>
          </div>
          <p className="mt-6 text-sm text-muted-foreground">
            No setup fees • Cancel anytime • 7-day money-back guarantee
          </p>
        </div>
      </section>
    </div>
  );
};
