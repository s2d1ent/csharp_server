<?php

use Twig\Environment;
use Twig\Error\LoaderError;
use Twig\Error\RuntimeError;
use Twig\Extension\SandboxExtension;
use Twig\Markup;
use Twig\Sandbox\SecurityError;
use Twig\Sandbox\SecurityNotAllowedTagError;
use Twig\Sandbox\SecurityNotAllowedFilterError;
use Twig\Sandbox\SecurityNotAllowedFunctionError;
use Twig\Source;
use Twig\Template;

/* preferences/header.twig */
class __TwigTemplate_35f0e58f9c7e48c76d42740b2ba8dd4e7c6b9dff982b43b5a6a9e10823016d78 extends \Twig\Template
{
    private $source;
    private $macros = [];

    public function __construct(Environment $env)
    {
        parent::__construct($env);

        $this->source = $this->getSourceContext();

        $this->parent = false;

        $this->blocks = [
        ];
    }

    protected function doDisplay(array $context, array $blocks = [])
    {
        $macros = $this->macros;
        // line 1
        echo "<div class=\"container-fluid\">
  <div class=\"row\">
    <ul id=\"user_prefs_tabs\" class=\"nav nav-pills m-2\">
      <li class=\"nav-item\">
        <a href=\"";
        // line 5
        echo PhpMyAdmin\Url::getFromRoute("/preferences/manage");
        echo "\" class=\"nav-link";
        echo (((($context["route"] ?? null) == "/preferences/manage")) ? (" active") : (""));
        echo "\">
          ";
        // line 6
        echo _gettext("Manage your settings");
        // line 7
        echo "        </a>
      </li>

      <li class=\"nav-item\">
        <a href=\"";
        // line 11
        echo PhpMyAdmin\Url::getFromRoute("/preferences/two-factor");
        echo "\" class=\"nav-link";
        echo (((($context["route"] ?? null) == "/preferences/two-factor")) ? (" active") : (""));
        echo "\">
          ";
        // line 12
        echo _gettext("Two-factor authentication");
        // line 13
        echo "        </a>
      </li>

      <li class=\"nav-item\">
        <a href=\"";
        // line 17
        echo PhpMyAdmin\Url::getFromRoute("/preferences/features");
        echo "\" class=\"nav-link";
        echo (((($context["route"] ?? null) == "/preferences/features")) ? (" active") : (""));
        echo "\">
          ";
        // line 18
        echo \PhpMyAdmin\Html\Generator::getIcon("b_tblops", _gettext("Features"));
        echo "
        </a>
      </li>

      <li class=\"nav-item\">
        <a href=\"";
        // line 23
        echo PhpMyAdmin\Url::getFromRoute("/preferences/sql");
        echo "\" class=\"nav-link";
        echo (((($context["route"] ?? null) == "/preferences/sql")) ? (" active") : (""));
        echo "\">
          ";
        // line 24
        echo \PhpMyAdmin\Html\Generator::getIcon("b_sql", _gettext("SQL queries"));
        echo "
        </a>
      </li>

      <li class=\"nav-item\">
        <a href=\"";
        // line 29
        echo PhpMyAdmin\Url::getFromRoute("/preferences/navigation");
        echo "\" class=\"nav-link";
        echo (((($context["route"] ?? null) == "/preferences/navigation")) ? (" active") : (""));
        echo "\">
          ";
        // line 30
        echo \PhpMyAdmin\Html\Generator::getIcon("b_select", _gettext("Navigation panel"));
        echo "
        </a>
      </li>

      <li class=\"nav-item\">
        <a href=\"";
        // line 35
        echo PhpMyAdmin\Url::getFromRoute("/preferences/main-panel");
        echo "\" class=\"nav-link";
        echo (((($context["route"] ?? null) == "/preferences/main-panel")) ? (" active") : (""));
        echo "\">
          ";
        // line 36
        echo \PhpMyAdmin\Html\Generator::getIcon("b_props", _gettext("Main panel"));
        echo "
        </a>
      </li>

      <li class=\"nav-item\">
        <a href=\"";
        // line 41
        echo PhpMyAdmin\Url::getFromRoute("/preferences/export");
        echo "\" class=\"nav-link";
        echo (((($context["route"] ?? null) == "/preferences/export")) ? (" active") : (""));
        echo "\">
          ";
        // line 42
        echo \PhpMyAdmin\Html\Generator::getIcon("b_export", _gettext("Export"));
        echo "
        </a>
      </li>

      <li class=\"nav-item\">
        <a href=\"";
        // line 47
        echo PhpMyAdmin\Url::getFromRoute("/preferences/import");
        echo "\" class=\"nav-link";
        echo (((($context["route"] ?? null) == "/preferences/import")) ? (" active") : (""));
        echo "\">
          ";
        // line 48
        echo \PhpMyAdmin\Html\Generator::getIcon("b_import", _gettext("Import"));
        echo "
        </a>
      </li>
    </ul>
  </div>

  ";
        // line 54
        if (($context["is_saved"] ?? null)) {
            // line 55
            echo "    ";
            echo call_user_func_array($this->env->getFilter('raw_success')->getCallable(), [_gettext("Configuration has been saved.")]);
            echo "
  ";
        }
        // line 57
        echo "
  ";
        // line 58
        if ( !($context["has_config_storage"] ?? null)) {
            // line 59
            echo "    ";
            ob_start(function () { return ''; });
            // line 60
            echo "      ";
            echo _gettext("Your preferences will be saved for current session only. Storing them permanently requires %sphpMyAdmin configuration storage%s.");
            // line 61
            echo "    ";
            $___internal_b0b285f82e9e956f2aeafce0d01cb24560f024eaf0860780d1469c466f4643e2_ = ('' === $tmp = ob_get_clean()) ? '' : new Markup($tmp, $this->env->getCharset());
            // line 59
            echo call_user_func_array($this->env->getFilter('notice')->getCallable(), [sprintf($___internal_b0b285f82e9e956f2aeafce0d01cb24560f024eaf0860780d1469c466f4643e2_, (("<a href=\"" . \PhpMyAdmin\Html\MySQLDocumentation::getDocumentationLink("setup", "linked-tables")) . "\">"), "</a>")]);
            // line 62
            echo "  ";
        }
    }

    public function getTemplateName()
    {
        return "preferences/header.twig";
    }

    public function isTraitable()
    {
        return false;
    }

    public function getDebugInfo()
    {
        return array (  180 => 62,  178 => 59,  175 => 61,  172 => 60,  169 => 59,  167 => 58,  164 => 57,  158 => 55,  156 => 54,  147 => 48,  141 => 47,  133 => 42,  127 => 41,  119 => 36,  113 => 35,  105 => 30,  99 => 29,  91 => 24,  85 => 23,  77 => 18,  71 => 17,  65 => 13,  63 => 12,  57 => 11,  51 => 7,  49 => 6,  43 => 5,  37 => 1,);
    }

    public function getSourceContext()
    {
        return new Source("", "preferences/header.twig", "C:\\OpenServer\\modules\\system\\html\\openserver\\phpmyadmin\\templates\\preferences\\header.twig");
    }
}
