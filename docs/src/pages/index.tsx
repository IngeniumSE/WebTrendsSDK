import clsx from 'clsx';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Layout from '@theme/Layout';
import HomepageFeatures from '@site/src/components/HomepageFeatures';
import Heading from '@theme/Heading';
import CodeBlock from '@theme/CodeBlock';

import styles from './index.module.css';

function HomepageHeader() {
  const {siteConfig} = useDocusaurusContext();
  return (
    <header className={clsx('hero hero--primary', styles.heroBanner)}>
      <div className="container">
        <Heading as="h1" className="hero__title">
          {siteConfig.title}
        </Heading>
        <p className="hero__subtitle">{siteConfig.tagline}</p>
        {/* <div className={styles.buttons}>
          <Link
            className="button button--secondary button--lg"
            to="/docs/intro">
            Docusaurus Tutorial - 5min ⏱️
          </Link>
        </div> */}
        <div className={styles.badges}>
          <a href="https://nuget.org/packages/WebTrendsSDK"><img src="https://img.shields.io/nuget/v/WebTrendsSDK" alt="NuGet" /></a>
          <a href="https://github.com/IngeniumSE/WebTrendsSDK"><img src="https://github.com/IngeniumSE/WebTrendsSDK/actions/workflows/main.yml/badge.svg" alt="GitHub: main" /></a>
          <a href="https://github.com/IngeniumSE/WebTrendsSDK"><img src="https://github.com/IngeniumSE/WebTrendsSDK/actions/workflows/release.yml/badge.svg" alt="Github: release" /></a>
          <a href="https://github.com/IngeniumSE/WebTrendsSDK"><img src="https://img.shields.io/github/stars/IngeniumSE/WebTrendsSDK" alt="GitHub stars" /></a>
          <a href="https://github.com/IngeniumSE/WebTrendsSDK"><img src="https://img.shields.io/github/issues/IngeniumSE/WebTrendsSDK" alt="GitHub issues" /></a>
          <a href="https://github.com/IngeniumSE/WebTrendsSDK"><img src="https://img.shields.io/github/license/IngeniumSE/WebTrendsSDK" alt="License" /></a>
        </div>
      </div>
    </header>
  );
}

export default function Home(): JSX.Element {
  const {siteConfig} = useDocusaurusContext();
  return (
    <Layout
      title={`${siteConfig.title}`}
      description="WebTrendsSDK - An open-source .NET Library for the WebTrends OTS REST API">
      <HomepageHeader />
      <main className="container">
        <div className={styles.mainContainer}>
          <div className="container">
            <h2>WebTrendsSDK - An open-source Unofficial SDK for integrating with the WebTrends OTS REST API</h2>
            <p>The WebTrendsSDK can be used to manage experiment participation in WebTrends testing.</p>
          </div>
          <div className="container">
            <div className="row">
              <div className={clsx('col col--6')}>
                <h3>Quick start (.NET Core &amp; .NET 5+)</h3>
                <p>Install the NuGet package:</p>
                <CodeBlock language="csharp">
                  dotnet add package WebTrendsSDK
                </CodeBlock>
                <p>Then add the WebTrendsSDK to your services:</p>
                <CodeBlock language="csharp">
                services.AddWebTrends();
                </CodeBlock>
                <p>Add your auth token to your config:</p>
                <CodeBlock language="json">
                  {`{
  "WebTrends": {
    "AccountId": "YOUR_ACCOUNT_ID",
    "KeyToken": "YOUR_AUTH_TOKEN",
    "WebsiteUrl": "https://www.example.com"
  }
}`}
                </CodeBlock>
                <p>Then use it in your code:</p>
                <CodeBlock language="csharp">
                  {`public class MyClass(IWebTrendsApiClient client)
{
  public async Task DoSomething()
  {
    var project = await client.Ots.Control.GetProjectAsync("ta_12345");
  }
}`}
                </CodeBlock>
              </div>
              <div className={clsx('col col--6')}>
                <h3>Open source</h3>
                <p>This project uses open-source components. Please consider contributing to, or sponsoring these amazing projects:</p>
                <ul>
                  <li><a href="https://github.com/dotnet" target="_blank">.NET Platform</a> by Microsoft and contributors</li>
                  <li><a href="https://github.com/benaadams/Ben.Demystifier" target="_blank">Ben.Demystifier</a> by Ben Adams</li>
                  <li><a href="https://github.com/facebook/docusaurus" target="_blank">Docusaurus</a> by Meta Platforms, inc, and contributors</li>
                  <li><a href="https://github.com/FluentValidation/FluentValidation" target="_blank">FluentValidation</a> by Jeremy Skinner and contributors</li>
                  <li><a href="https://github.com/adamralph/minver" target="_blank">MinVer</a> by Adam Ralph and constributors</li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </main>
    </Layout>
  );
}
