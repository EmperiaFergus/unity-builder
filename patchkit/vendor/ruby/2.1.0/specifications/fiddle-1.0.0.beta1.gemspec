# -*- encoding: utf-8 -*-
# stub: fiddle 1.0.0.beta1 ruby lib

Gem::Specification.new do |s|
  s.name = "fiddle"
  s.version = "1.0.0.beta1"

  s.required_rubygems_version = Gem::Requirement.new("> 1.3.1") if s.respond_to? :required_rubygems_version=
  s.require_paths = ["lib"]
  s.authors = ["Aaron Patterson", "SHIBATA Hiroshi"]
  s.bindir = "exe"
  s.date = "2017-04-05"
  s.description = "A libffi wrapper for Ruby."
  s.email = ["aaron@tenderlovemaking.com", "hsbt@ruby-lang.org"]
  s.homepage = "https://github.com/ruby/fiddle"
  s.licenses = ["MIT"]
  s.rubygems_version = "2.2.5"
  s.summary = "A libffi wrapper for Ruby."

  s.installed_by_version = "2.2.5" if s.respond_to? :installed_by_version

  if s.respond_to? :specification_version then
    s.specification_version = 4

    if Gem::Version.new(Gem::VERSION) >= Gem::Version.new('1.2.0') then
      s.add_development_dependency(%q<bundler>, [">= 0"])
      s.add_development_dependency(%q<rake>, [">= 0"])
      s.add_development_dependency(%q<minitest>, ["~> 4.0"])
      s.add_development_dependency(%q<rake-compiler>, [">= 0"])
    else
      s.add_dependency(%q<bundler>, [">= 0"])
      s.add_dependency(%q<rake>, [">= 0"])
      s.add_dependency(%q<minitest>, ["~> 4.0"])
      s.add_dependency(%q<rake-compiler>, [">= 0"])
    end
  else
    s.add_dependency(%q<bundler>, [">= 0"])
    s.add_dependency(%q<rake>, [">= 0"])
    s.add_dependency(%q<minitest>, ["~> 4.0"])
    s.add_dependency(%q<rake-compiler>, [">= 0"])
  end
end
